using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using Shuttle.Packager.WebApi.Repositories;
using Shuttle.Packager.WebApi.Services;

namespace Shuttle.Packager.WebApi.Endpoints;

public static class ProjectEndpoints
{
    private static async Task<(string Log, bool Failed)> ExecuteAsync(string arguments)
    {
        using var process = new Process
        {
            StartInfo = new()
            {
                Arguments = arguments,
                FileName = "dotnet",
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        var log = new StringBuilder();

        void Append(string? data)
        {
            if (data == null)
            {
                return;
            }

            lock (log)
            {
                log.AppendLine(data);
            }
        }

        process.OutputDataReceived += (_, args) => Append(args.Data);
        process.ErrorDataReceived += (_, args) => Append(args.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        return (log.ToString(), process.ExitCode != 0);
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

            var (log, failed) = await ExecuteAsync($"build {project.FilePath} --configuration {model.Configuration}");

            return Results.Ok(new
            {
                Log = log,
                Failed = failed
            });
        });

        app.MapPatch("/projects/{id:guid}/pack", async (IProjectRepository repository, Guid id, PackageOptionsModel model) =>
        {
            var project = await repository.GetAsync(id);

            var (log, failed) = await ExecuteAsync($"pack {project.FilePath} --configuration {model.Configuration}");

            return Results.Ok(new
            {
                Log = log,
                Failed = failed
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
            var (packLog, packFailed) = await ExecuteAsync($"pack {project.FilePath}");

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

            var (nugetLog, nugetFailed) = await ExecuteAsync(command);

            return Results.Ok(new
            {
                Log = packLog + "\n" + nugetLog,
                Failed = nugetFailed
            });
        });

        app.MapPatch("/projects/{id:guid}/property", async (IProjectRepository repository, Guid id, PropertyPatchModel model) =>
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

        app.MapGet("/projects/{id:guid}/package-version", async (IOptions<PackagerOptions> options, IProjectRepository repository, IPackageVersionService packageVersionService, Guid id, string? packageSourceName, CancellationToken cancellationToken) =>
        {
            PackageSourceOptions? packageSourceOptions = null;

            if (!string.IsNullOrWhiteSpace(packageSourceName))
            {
                packageSourceOptions = options.Value.PackageSources.FirstOrDefault(item => item.Name.Equals(packageSourceName, StringComparison.OrdinalIgnoreCase));

                if (packageSourceOptions == null)
                {
                    return Results.BadRequest($"Unknown package source name '{packageSourceName}'.");
                }
            }

            var project = await repository.GetAsync(id);

            try
            {
                return Results.Ok(new
                {
                    Version = await packageVersionService.GetLatestVersionAsync(project.Name, packageSourceOptions, cancellationToken)
                });
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (ApplicationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (FatalProtocolException ex)
            {
                return Results.Problem(detail: ExceptionUtilities.DisplayMessage(ex), title: $"Could not reach package source '{packageSourceName ?? "nuget.org"}'.", statusCode: 502);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ExceptionUtilities.DisplayMessage(ex), title: $"Could not determine the latest version for '{project.Name}'.", statusCode: 500);
            }
        });

        return app;
    }
}