using Consumers.Core.Application.Abstractions.Repositories;
using Consumers.Core.Application.Abstractions.Services;
using Consumers.Core.Application.Behaviors;
using Consumers.Core.Application.Commands;
using Consumers.Core.Application.Services;
using Consumers.Infrastructure.Persistence;
using Consumers.Infrastructure.Persistence.Repositories;
using Consumers.Presentation.PostProcessors;
using Consumers.Presentation.PreProcessors;
using Consumers.WebApi.Managers;
using Consumers.WebApi.Utils;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Update = Consumers.Presentation.Endpoints.Orders.Commands.Update;

var builder = WebApplication.CreateSlimBuilder(args);

var databaseConnectionString = builder.Configuration.GetConnectionString("App");

builder.Host.UseSerilog((_, _, configuration) =>
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.MySQL(databaseConnectionString)
        .Enrich.FromLogContext());

var mysqlServerVersion = ServerVersion.AutoDetect(databaseConnectionString);

var domainAssembly = typeof(Order).Assembly;
var applicationAssembly = typeof(Create).Assembly;
var presentationAssembly = typeof(Update).Assembly;
var persistenceAssembly = typeof(ApplicationContext).Assembly;

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
    .AddScoped<IConsumersRepository, ConsumersRepository>()
    .AddScoped<IConsumersService, ConsumersService>()
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
                s.Title = "Consumers";
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
