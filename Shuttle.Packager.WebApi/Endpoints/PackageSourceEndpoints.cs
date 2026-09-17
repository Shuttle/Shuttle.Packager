using Microsoft.Extensions.Options;

namespace Shuttle.Packager.WebApi.Endpoints;

public static class PackageSourceEndpoints
{
    public static WebApplication MapPackageSourceEndpoints(this WebApplication app)
    {
        app.MapGet("/package-sources", (IOptions<PackagerOptions> packagerOptions) => Results.Ok(packagerOptions.Value.PackageSources.Select(item => new PackageSourceModel { Name = item.Name, Url = item.Url })));

        return app;
    }
}