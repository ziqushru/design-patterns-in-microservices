using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Core.Application.Abstractions.Services;
using Core.Application.Abstractions.Repositories;
using Presentation.PostProcessors;
using Core.Application.Behaviors;
using Presentation.PreProcessors;
using WebApi.Utils;
using WebApi.Managers;
using Core.Application.Services;

var builder = WebApplication.CreateSlimBuilder(args);

var databaseConnectionString = builder.Configuration.GetConnectionString("App");

builder.Host.UseSerilog((_, _, configuration) =>
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.MySQL(databaseConnectionString)
        .Enrich.FromLogContext());

var mysqlServerVersion = ServerVersion.AutoDetect(databaseConnectionString);

var domainAssembly = typeof(Order).Assembly;
var applicationAssembly = typeof(Core.Application.Commands.Create).Assembly;
var presentationAssembly = typeof(Presentation.Endpoints.Commands.Update).Assembly;
var persistenceAssembly = typeof(Infrastructure.Persistence.ApplicationContext).Assembly;

ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

builder.Services
    .AddDbContext<ApplicationContext>(applicationContextOptions =>
    {
        applicationContextOptions.UseLazyLoadingProxies();
        applicationContextOptions.EnableSensitiveDataLogging();

        applicationContextOptions.UseMySql(databaseConnectionString,
            mysqlServerVersion,
            options =>
            {
                options.EnableRetryOnFailure();
                options.EnableStringComparisonTranslations();
                options.MigrationsAssembly(persistenceAssembly.FullName);
            });
    })
    .AddHttpContextAccessor()
    .AddMediatR(cfg =>
    {
        cfg.AutoRegisterRequestProcessors = true;
        cfg.RegisterServicesFromAssembly(applicationAssembly);
    })
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
    .AddAutoMapper(new[] { presentationAssembly, applicationAssembly })
    .AddValidatorsFromAssembly(applicationAssembly, ServiceLifetime.Transient, includeInternalTypes: true)
    .AddFastEndpoints(options =>
    {
        options.DisableAutoDiscovery = true;
        options.Assemblies = [presentationAssembly];
    })
    .AddScoped<IOrdersRepository, OrdersRepository>()
    .AddScoped<IOrdersService, OrdersService>()
    .AddCors(
        options => options.AddPolicy("AllowAll", builder =>
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    builder.Services
        .SwaggerDocument(o =>
        {
            o.AutoTagPathSegmentIndex = 0;
            o.MaxEndpointVersion = 1;
            o.DocumentSettings = s =>
            {
                s.DocumentName = "v1.0.0";
                s.Title = "Payments";
                s.Version = "v1.0.0";
            };
        });
}

if (builder.Environment.IsStaging() || builder.Environment.IsProduction())
{
    builder.Services.AddResponseCaching();
}

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
    options.ValidateOnBuild = true;
});

var app = builder.Build();

app.UseMigrationsManager()
    .UseCors("AllowAll")
    .UseFastEndpoints(c =>
    {
        c.Versioning.Prefix = "v";
        c.Endpoints.Configurator = ep =>
        {
            ep.PreProcessor<ExceptionPreProcessor>(Order.Before);
            ep.PostProcessor<ExceptionPostProcessor>(Order.Before);
        };
    });

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwaggerGen()
        .UseSerilogRequestLogging();
}

if (app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseResponseCaching();
}

await app.RunAsync();
