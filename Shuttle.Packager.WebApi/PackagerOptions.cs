namespace Shuttle.Packager.WebApi;

public class PackagerOptions
{
    public const string SectionName = "Packager";

    public required string BaseFolder { get; set; }
    public string FolderIgnoreExpression { get; set; } = "(node_modules|bin|obj)";
    public string VisualStudioPath { get; set; } = string.Empty;
}