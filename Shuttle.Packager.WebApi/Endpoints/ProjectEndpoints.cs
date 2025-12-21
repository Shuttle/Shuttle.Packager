using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Shuttle.Packager.WebApi.Repositories;

namespace Shuttle.Packager.WebApi.Endpoints;

public static class ProjectEndpoints
{
    private static async Task<string> ExecuteAsync(string arguments)
    {
        var process = new Process
        {
            StartInfo = new()
            {
                Arguments = arguments,
                FileName = "dotnet",
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        var log = new StringBuilder();

        process.OutputDataReceived += (_, args) =>
        {
            log.AppendLine(args.Data ?? string.Empty);
        };

        process.Start();
        process.BeginOutputReadLine();

        while (!process.HasExited)
        {
            await Task.Delay(100);
        }

        process.CancelOutputRead();

        var logText = log.ToString();
        return logText;
    }

    private static ProjectModel Map(Project project)
    {
        return new()
        {
            Id = project.Id,
            Name = project.Name,
            FilePath = project.FilePath,
            Folder = project.Folder,
            Version = project.Version,
            PackageReferences = project.PackageReferences
                .Select(item => new ProjectModel.PackageReference
                {
                    Name = item.Name, Version = item.Version
                })
                .ToList()
        };
    }

    public static WebApplication MapProjectEndpoints(this WebApplication app)
    {
        app.MapGet("/projects", async (IProjectRepository repository) => (await repository.GetAsync()).Select(Map));

        app.MapPatch("/projects/load", async (IOptions<PackagerOptions> options, IProjectRepository repository) =>
        {
            var baseFolder = options.Value.BaseFolder;
            var folderIgnoreExpression = options.Value.FolderIgnoreExpression;

            var projects = Directory.EnumerateFiles(baseFolder, "*.csproj", SearchOption.AllDirectories)
                .Where(file => !Regex.IsMatch(Path.GetDirectoryName(file)!, folderIgnoreExpression, RegexOptions.IgnoreCase))
                .Select(file => new Project(file));

            await repository.SaveAsync(projects);

            return Results.Ok("Projects refreshed and saved successfully.");
        });

        app.MapPatch("/projects/{id:guid}/build", async (IProjectRepository repository, Guid id, PackageOptionsModel model) =>
        {
            var project = await repository.GetAsync(id);

            var log = await ExecuteAsync($"build {project.FilePath} --configuration {model.Configuration}");

            return Results.Ok(new
            {
                Log = log,
                Failed = log.Contains("failed", StringComparison.InvariantCultureIgnoreCase)
            });
        });

        app.MapPatch("/projects/{id:guid}/pack", async (IProjectRepository repository, Guid id, PackageOptionsModel model) =>
        {
            var project = await repository.GetAsync(id);

            var log = await ExecuteAsync($"pack {project.FilePath} --configuration {model.Configuration}");

            return Results.Ok(new
            {
                Log = log,
                Failed = log.Contains("failed", StringComparison.InvariantCultureIgnoreCase)
            });
        });

        app.MapPatch("/projects/{id:guid}/push", async (IOptions<PackagerOptions> options, IProjectRepository repository, Guid id, PackageOptionsModel model) =>
        {
            var packageSourceName = string.Empty;
            var packageSourceKey = string.Empty;

            if (!string.IsNullOrWhiteSpace(model.PackageSourceName))
            {
                var packageSource = options.Value.PackageSources.FirstOrDefault(item => item.Name.Equals(model.PackageSourceName));

                if (packageSource == null)
                {
                    return Results.BadRequest($"Unknown package source name '{model.PackageSourceName}'.");
                }

                packageSourceName = packageSource.Name;
                packageSourceKey = packageSource.Key;
            }

            var project = await repository.GetAsync(id);
            var packLog = await ExecuteAsync($"pack {project.FilePath}");
            var packFailed = packLog.Contains("failed", StringComparison.InvariantCultureIgnoreCase);

            if (packFailed)
            {
                return Results.Ok(new
                {
                    Log = packLog,
                    Failed = packFailed
                });
            }

            var command = $"nuget push {project.GetPackageFilePath("Release")}";

            if (!string.IsNullOrWhiteSpace(packageSourceName))
            {
                command += $" -s {packageSourceName}";
            }

            if (!string.IsNullOrWhiteSpace(packageSourceKey))
            {
                command += $" -k {packageSourceKey}";
            }

            var nugetLog = await ExecuteAsync(command);
            var nugetFailed = nugetLog.Contains("error:", StringComparison.InvariantCultureIgnoreCase);

            return Results.Ok(new
            {
                Log = packLog + "\n" + nugetLog,
                Failed = nugetFailed
            });
        });

        app.MapPatch("/projects/{id:guid}/property", async (IOptions<PackagerOptions> options, IProjectRepository repository, Guid id, PropertyPatchModel model) =>
        {
            var project = await repository.GetAsync(id);

            switch (model.Name.ToUpperInvariant())
            {
                case "VERSION":
                {
                    if (await project.SetVersionAsync(model.Value))
                    {
                        return Results.Ok();
                    }

                    break;
                }
            }

            return Results.BadRequest();
        });

        app.MapPatch("/projects/{id:guid}/open", async (IOptions<PackagerOptions> options, IProjectRepository repository, Guid id) =>
        {
            var project = await repository.GetAsync(id);

            if (string.IsNullOrEmpty(project.GetSolutionPath()))
            {
                return Results.Problem(new()
                {
                    Detail = $"Cannot find a solution path for project '{project.Name}'."
                });
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = options.Value.VisualStudioPath,
                Arguments = project.GetSolutionPath()
            };

            Process.Start(startInfo);

            return Results.Ok();
        });

        app.MapGet("/projects/{id:guid}/nuget-version", async (IHttpClientFactory httpClientFactory, IProjectRepository repository, Guid id) =>
        {
            var httpClient = httpClientFactory.CreateClient("nuget");

            var project = await repository.GetAsync(id);

            using var response = await httpClient.GetAsync(new Uri($"https://api.nuget.org/v3-flatcontainer/{project.Name}/index.json"));

            var version = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                using var document = JsonDocument.Parse(response.Content.ReadAsStringAsync().Result);

                version = document.RootElement.GetProperty("versions").EnumerateArray()
                    .LastOrDefault()
                    .ToString();
            }

            return Results.Ok(new
            {
                NugetVersion = version
            });
        });

        return app;
    }
}