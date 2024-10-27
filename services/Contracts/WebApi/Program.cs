using System;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Application.Abstractions.Services;
using Contracts.Core.Application.Behaviors;
using Contracts.Core.Application.Commands;
using Contracts.Core.Application.Services;
using Contracts.Infrastructure.Persistence;
using Contracts.Infrastructure.Persistence.Repositories;
using Contracts.MessageBus.Consumers;
using Contracts.Presentation.PostProcessors;
using Contracts.Presentation.PreProcessors;
using Contracts.WebApi.Managers;
using Contracts.WebApi.Utils;
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
using Update = Contracts.Presentation.Endpoints.Orders.Commands.Update;

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
var messageBusAssembly = typeof(ConsumerCreated).Assembly;

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
    .AddAutoMapper(new[] { presentationAssembly, applicationAssembly, messageBusAssembly })
    .AddValidatorsFromAssembly(applicationAssembly, ServiceLifetime.Transient, includeInternalTypes: true)
    .AddFastEndpoints(options =>
    {
        options.DisableAutoDiscovery = true;
        options.Assemblies = [presentationAssembly];
    })
    .AddMassTransit(busConfigurator =>
    {
        busConfigurator.SetKebabCaseEndpointNameFormatter();
        
        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
            {
                h.Username(builder.Configuration["MessageBroker:Username"]!);
                h.Password(builder.Configuration["MessageBroker:Password"]!);
            });

            configurator.ConfigureEndpoints(context);
        });
    })
    .AddScoped<IContractsRepository, ContractsRepository>()
    .AddScoped<IContractsService, ContractsService>()
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
                s.Title = "Contracts";
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
