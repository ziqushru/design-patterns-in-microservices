using System;
using Providers.Core.Application.Abstractions.Repositories;
using Providers.Core.Application.Abstractions.Services;
using Providers.Core.Application.Behaviors;
using Providers.Core.Application.Commands;
using Providers.Core.Application.Services;
using Providers.Infrastructure.Persistence;
using Providers.Infrastructure.Persistence.Repositories;
using Providers.Presentation.PostProcessors;
using Providers.Presentation.PreProcessors;
using Providers.WebApi.Managers;
using Providers.WebApi.Utils;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Update = Providers.Presentation.Endpoints.Orders.Commands.Update;

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
    .AddMassTransit(busConfigurator =>
    {
        busConfigurator.UsingRabbitMq((context, configurator)=>
        {
            configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
            {
                h.Username(builder.Configuration["MessageBroken:Username"]!);
                h.Password(builder.Configuration["MessageBroken:Password"]!);
            });
            
            configurator.ConfigureEndpoints(context);
        });  
    })    .AddScoped<IProvidersRepository, ProvidersRepository>()
    .AddScoped<IProvidersService, ProvidersService>()
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
                s.Title = "Providers";
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
