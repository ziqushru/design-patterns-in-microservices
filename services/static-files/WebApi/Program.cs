using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDirectoryBrowser();

var app = builder.Build();

var fileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles"),
    ExclusionFilters.Sensitive);

var requestPath = new PathString("/static-files");

app.UseDefaultFiles()
    .UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = fileProvider,
        RequestPath = requestPath
    })
    .UseDirectoryBrowser(new DirectoryBrowserOptions
    {
        FileProvider = fileProvider,
        RequestPath = requestPath
    });

await app.RunAsync();
