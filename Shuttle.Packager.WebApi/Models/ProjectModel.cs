namespace Shuttle.Packager.WebApi;

public class ProjectModel
{
    public class PackageReference
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public List<PackageReference> PackageReferences { get; set; } = [];
}