using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shuttle.Packager.WebApi.Repositories;

namespace Shuttle.Packager.WebApi.Endpoints;

public static class ProjectEndpoints
{
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
            var log = await ExecuteAsync("build", repository, id, model);

            return Results.Ok(new
            {
                Log = log,
                Failed = log.Contains("failed", StringComparison.InvariantCultureIgnoreCase)
            });
        });

        app.MapPatch("/projects/{id:guid}/pack", async (IProjectRepository repository, Guid id, PackageOptionsModel model) =>
        {
            var log = await ExecuteAsync("pack", repository, id, model);

            return Results.Ok(new
            {
                Log = log,
                Failed = log.Contains("failed", StringComparison.InvariantCultureIgnoreCase)
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
                return Results.Problem(new ProblemDetails
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

        return app;
    }

    private static async Task<string> ExecuteAsync(string command, IProjectRepository repository, Guid id, PackageOptionsModel model)
    {
        var project = await repository.GetAsync(id);

        var process = new Process
        {
            StartInfo = new()
            {
                Arguments = $"{command} {project.Path} --configuration {model.Configuration}",
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
            Version = project.Version
        };
    }
}