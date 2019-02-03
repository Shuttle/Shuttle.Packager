using System;
using System.Text.RegularExpressions;

namespace Shuttle.Packager
{
    public class SemanticVersion
    {
        private readonly Regex _semverExpression =
            new Regex(@"(?<major>\d*)\.(?<minor>\d*)\.(?<patch>\d*)", RegexOptions.IgnoreCase);

        public SemanticVersion(string version)
        {
            var match = _semverExpression.Match(version);

            if (!match.Success)
            {
                throw new InvalidOperationException(
                    $"Version string '{version}' does not conform to a semanatic version format.");
            }

            Major = int.Parse(match.Groups["major"].Value);
            Minor = int.Parse(match.Groups["minor"].Value);
            Patch = int.Parse(match.Groups["patch"].Value);
        }

        private SemanticVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }

        public SemanticVersion Copy()
        {
            return new SemanticVersion(Major, Minor, Patch);
        }

        public bool IsEqualTo(SemanticVersion other)
        {
            return Major == other.Major
                   &&
                   Minor == other.Minor
                   &&
                   Patch == other.Patch;
        }

        public void IncreaseMajor()
        {
            Major++;
            Minor = 0;
            Patch = 0;
        }

        public void IncreaseMinor()
        {
            Minor++;
            Patch = 0;
        }

        public void IncreasePatch()
        {
            Patch++;
        }

        public string Formatted()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}