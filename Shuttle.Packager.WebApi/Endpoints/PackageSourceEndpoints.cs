using Microsoft.Extensions.Options;

namespace Shuttle.Packager.WebApi.Endpoints;

public static class PackageSourceEndpoints
{
    public static WebApplication MapPackageSourceEndpoints(this WebApplication app)
    {
        app.MapGet("/package-sources", (IOptions<PackagerOptions> packagerOptions) => Results.Ok((object?)packagerOptions.Value.PackageSources));

        return app;
    }
}