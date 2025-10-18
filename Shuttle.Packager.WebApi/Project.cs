using System.Text.RegularExpressions;
using Shuttle.Core.Contract;

namespace Shuttle.Packager.WebApi
{
    public class Project
    {
        private string _solutionPath = string.Empty;
        private readonly Regex _versionExpression = new("<Version>(?<version>.*?)</Version>");

        public Project(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Project file '{path}' could not be found.");
            }

            FilePath = Guard.AgainstEmpty(path);
            Name = Path.GetFileNameWithoutExtension(path);

            Read();
        }

        private void Read()
        {
            var contents = File.ReadAllText(FilePath);

            var match = _versionExpression.Match(contents);

            if (match.Success)
            {
                Version = match.Groups["version"].Value;
            }
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string FilePath { get; }
        public string Name { get; private set; }
        public string Version { get; set; } = string.Empty;

        public string GetSolutionPath()
        {
            var path = Path.GetDirectoryName(FilePath);

            while (string.IsNullOrEmpty(_solutionPath))
            {
                if (path == null)
                {
                    return string.Empty;
                }

                var files = Directory.GetFiles(path, "*.sln");

                if (files.Any())
                {
                    _solutionPath = files[0];
                    break;
                }

                path = Path.GetDirectoryName(path);
            }

            return _solutionPath;
        }

        public async Task<bool> SetVersionAsync(string version)
        {
            var contents = await File.ReadAllTextAsync(FilePath);

            var match = _versionExpression.Match(contents);

            if (!match.Success)
            {
                return false;
            }

            var updatedContents = contents.Replace(match.Value, $"<Version>{version}</Version>");
            await File.WriteAllTextAsync(FilePath, updatedContents);
            Version = version;

            return true;
        }

        public string GetPackageFilePath(string configuration)
        {
            var path = Path.Combine(Guard.AgainstEmpty(Path.GetDirectoryName(FilePath)), "bin", Guard.AgainstEmpty(configuration), $"{Name}.{Version}.nupkg");

            return !File.Exists(path) ? throw new ApplicationException($"Could not find package file '{path}'.") : path;
        }
    }
}