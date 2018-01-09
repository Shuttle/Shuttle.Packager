using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Shuttle.Packager
{
    internal class Package
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

            RenderVersion();
        }

        public string Name => _item.Text;
        public string ProjectPath { get; }
        public SemanticVersion CurrentVersion { get; private set; }
        public SemanticVersion BuildVersion { get; private set; }
        public string MSBuildPath { get; }
        public string BuildLog { get; private set; }

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
            _item.SubItems[1].Text = CurrentVersion.IsEqualTo(BuildVersion)
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
    }
}