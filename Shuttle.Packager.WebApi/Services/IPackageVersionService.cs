namespace Shuttle.Packager.WebApi.Services;

public interface IPackageVersionService
{
    Task<string> GetLatestVersionAsync(string packageId, PackageSourceOptions? packageSourceOptions, CancellationToken cancellationToken = default);
}
