using Microsoft.Extensions.Options;

namespace Shuttle.Packager.WebApi.Endpoints;

public static class PackageSourceEndpoints
{
    public static WebApplication MapPackageSourceEndpoints(this WebApplication app)
    {
        app.MapGet("/package-sources", (IOptions<PackagerOptions> packagerOptions) =>
        {
            var result = packagerOptions.Value.PackageSources;

            if (!result.Any(item => item.Name.Equals("Default")))
            {
                result.Add(new()
                {
                    Name = "Default"
                });
            }

            return Results.Ok(result);
        });

        return app;
    }
}