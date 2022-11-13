using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace Shuttle.Packager
{
    public class SemanticVersion
    {
        private readonly Regex _expression =
            new Regex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$");
        private readonly Regex _semverExpression =
            new Regex(@"(?<major>\d*)\.(?<minor>\d*)\.(?<patch>\d*)", RegexOptions.IgnoreCase);

        public SemanticVersion(string version)
        {
            var exception = new ArgumentException($"Argument '{nameof(version)}' is not a valid semantic version.");

            if (string.IsNullOrWhiteSpace(version))
            {
                throw exception;
            }

            var match = _expression.Match(version);

            if (!match.Success)
            {
                throw exception;
            }

            Major = int.Parse(match.Groups["major"].Value);
            Minor = int.Parse(match.Groups["minor"].Value);
            Patch = int.Parse(match.Groups["patch"].Value);

            if (match.Groups["buildmetadata"].Success)
            {
                BuildMetadata = match.Groups["buildmetadata"].Value;
            }

            if (match.Groups["prerelease"].Success)
            {
                Prerelease = match.Groups["prerelease"].Value;
            }
        }

        private SemanticVersion(int major, int minor, int patch, string prerelease, string buildMetadata)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Prerelease = prerelease ?? string.Empty;
            BuildMetadata = buildMetadata ?? string.Empty;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public string Prerelease { get; private set; } = string.Empty;
        public string BuildMetadata { get; } = string.Empty;
        
        public SemanticVersion Copy()
        {
            return new SemanticVersion(Major, Minor, Patch, Prerelease, BuildMetadata);
        }

        public bool IsEqualTo(SemanticVersion other)
        {
            return Major == other.Major
                   &&
                   Minor == other.Minor
                   &&
                   Patch == other.Patch
                   &&
                   Prerelease.Equals(other.Prerelease)
                   &&
                   BuildMetadata.Equals(other.BuildMetadata);
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

        public void SetPrerelease(string prerelease)
        {
            Prerelease = prerelease ?? string.Empty;
        }

        public string Formatted()
        {
            return $"{Major}.{Minor}.{Patch}{(string.IsNullOrEmpty(Prerelease) ? string.Empty : $"-{Prerelease}")}";
        }
    }
}