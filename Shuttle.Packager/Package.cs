using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Contract;

namespace Shuttle.Packager
{
    public enum PackageVersion
    {
        v1 = 1,
        v2 = 2
    }

    public class Package
    {
        private readonly ListViewItem _item;
        private string _solutionPath;

        public Package(ListViewItem item, string projectPath, string msbuildPath,
            SemanticVersion currentVersion)
        {
            _item = item;
            ProjectPath = projectPath;
            CurrentVersion = currentVersion;
            BuildVersion = CurrentVersion.Copy();
            MSBuildPath = msbuildPath;
            BuildLog = string.Empty;

            PackageVersion = msbuildPath.Contains(@"\.package\")
                ? PackageVersion.v2
                : PackageVersion.v1;

            RenderVersion();
        }

        public string Name => _item.Text;
        public string ProjectPath { get; }
        public SemanticVersion CurrentVersion { get; private set; }
        public SemanticVersion BuildVersion { get; private set; }
        public string MSBuildPath { get; }
        public string BuildLog { get; private set; }
        public PackageVersion PackageVersion { get; private set; }

        public bool Checked
        {
            get => _item.Checked;
            set => _item.Checked = value;
        }

        public Package IncreaseMajor()
        {
            BuildVersion.IncreaseMajor();

            RenderVersion();

            return this;
        }

        public Package IncreaseMinor()
        {
            BuildVersion.IncreaseMinor();

            RenderVersion();

            return this;
        }

        public Package IncreasePatch()
        {
            BuildVersion.IncreasePatch();

            RenderVersion();

            return this;
        }

        private void RenderVersion()
        {
            _item.SubItems["Version"].Text = CurrentVersion.IsEqualTo(BuildVersion)
                ? CurrentVersion.Formatted()
                : CurrentVersion.Formatted() + " => " + BuildVersion.Formatted();
        }

        public void ResetVersion()
        {
            BuildVersion = CurrentVersion.Copy();

            RenderVersion();
        }

        public void CaptureBuildLog(string text)
        {
            BuildLog = text;
        }

        public void ApplyBuildVersion()
        {
            CurrentVersion = BuildVersion.Copy();

            RenderVersion();
        }

        public bool HasFailed()
        {
            return !BuildLog.ToLower().Contains("build succeeded.");
        }

        public string GetSolutionPath()
        {
            var path = Path.GetDirectoryName(ProjectPath);

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

        public void OpenSolution()
        {
            if (string.IsNullOrEmpty(GetSolutionPath()))
            {
                return;
            }

            Process.Start(GetSolutionPath());
        }

        public void ShowUsage(string version)
        {
            _item.SubItems["Usage"].Text = version;
        }

        public string GetTarget(string target)
        {
            switch (target.ToLowerInvariant())
            {
                case "":
                {
                    return PackageVersion == PackageVersion.v1 ? string.Empty : "Push";
                    }
                case "package":
                {
                    return PackageVersion == PackageVersion.v1 ? target : "Bump";
                }

            }

            return string.Empty;
        }
    }
}