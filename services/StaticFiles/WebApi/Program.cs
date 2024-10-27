using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDirectoryBrowser();

var app = builder.Build();

app.MapPost("/create",
    async (HttpRequest request) =>
        {
            var file = request.Form.Files[0];
            var filePath = request.Form["filePath"];

            if (file is null || file.Length == 0)
            {
                return Results.BadRequest("No file uploaded");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");

            uploadPath = uploadPath + "/" + filePath;

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fullFilePath = Path.Combine(uploadPath, file.FileName);

            using var stream = new FileStream(fullFilePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return Results.Ok();
        });

var fileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles"),
    ExclusionFilters.Sensitive);

var requestPath = new PathString("/get");

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
