using System.Text.RegularExpressions;

namespace Shuttle.Packager
{
    public class DependentPackageMatch
    {
        public Package Package { get; }
        public Match Match { get; }
        public string ProjectContent { get; }

        public DependentPackageMatch(Package package, Match match, string projectContent)
        {
            Package = package;
            Match = match;
            ProjectContent = projectContent;
        }

        public void ShowUsage()
        {
            Package.ShowUsage(GetVersion());
        }

        public string GetVersion()
        {
            return Match.Groups["version"].Value;
        }
    }
}