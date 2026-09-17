using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Shuttle.Packager.WebApi.Services;

public class PackageVersionService(IOptions<PackagerOptions> packagerOptions) : IPackageVersionService
{
    private const string DefaultSourceName = "nuget.org";
    private const string DefaultSourceUrl = "https://api.nuget.org/v3/index.json";

    private readonly ConcurrentDictionary<string, Lazy<SourceRepository>> _sourceRepositories = new(StringComparer.OrdinalIgnoreCase);
    private readonly Lazy<IReadOnlyList<PackageSource>> _settingsPackageSources = new(() => GetSettingsPackageSources(packagerOptions.Value.BaseFolder));

    public async Task<string> GetLatestVersionAsync(string packageId, PackageSourceOptions? packageSourceOptions, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(packageId);

        var sourceRepository = GetSourceRepository(packageSourceOptions);
        var findPackageByIdResource = await sourceRepository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);

        using var sourceCacheContext = new SourceCacheContext
        {
            NoCache = true
        };

        var versions = await findPackageByIdResource.GetAllVersionsAsync(packageId, sourceCacheContext, NullLogger.Instance, cancellationToken);

        return versions?.OrderByDescending(item => item).FirstOrDefault()?.ToNormalizedString() ?? string.Empty;
    }

    private SourceRepository GetSourceRepository(PackageSourceOptions? packageSourceOptions)
    {
        var name = string.IsNullOrWhiteSpace(packageSourceOptions?.Name) ? DefaultSourceName : packageSourceOptions.Name;

        return _sourceRepositories.GetOrAdd(name, _ => new(() => Repository.Factory.GetCoreV3(GetPackageSource(packageSourceOptions)))).Value;
    }

    private PackageSource GetPackageSource(PackageSourceOptions? packageSourceOptions)
    {
        if (packageSourceOptions == null)
        {
            return new(DefaultSourceUrl, DefaultSourceName);
        }

        var packageSource = _settingsPackageSources.Value.FirstOrDefault(item => item.Name.Equals(packageSourceOptions.Name, StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrWhiteSpace(packageSourceOptions.Url) && item.Source.Equals(packageSourceOptions.Url, StringComparison.OrdinalIgnoreCase)));

        if (packageSource != null)
        {
            return packageSource;
        }

        if (string.IsNullOrWhiteSpace(packageSourceOptions.Url))
        {
            throw new ApplicationException($"Package source '{packageSourceOptions.Name}' was not found in the NuGet configuration and does not have a 'Url' configured.");
        }

        return new(packageSourceOptions.Url, packageSourceOptions.Name);
    }

    private static IReadOnlyList<PackageSource> GetSettingsPackageSources(string baseFolder)
    {
        var settings = Settings.LoadDefaultSettings(Directory.Exists(baseFolder) ? baseFolder : null);

        return new PackageSourceProvider(settings).LoadPackageSources().ToList();
    }
}
