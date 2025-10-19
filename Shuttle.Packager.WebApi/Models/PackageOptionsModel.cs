namespace Shuttle.Packager.WebApi;

public class PackageOptionsModel
{
    public string Configuration { get; set; } = "Debug";
    public string PackageSourceName { get; set; } = string.Empty;
}