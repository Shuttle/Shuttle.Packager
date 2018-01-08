using System.Windows.Forms;

namespace Shuttle.Packager
{
    internal class Package
    {
        private readonly ListViewItem _item;

        public Package(ListViewItem item, string projectPath, string msbuildPath, SemanticVersion currentVersion)
        {
            _item = item;
            ProjectPath = projectPath;
            CurrentVersion = currentVersion;
            BuildVersion = CurrentVersion.Copy();
            MSBuildPath = msbuildPath;

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
    }
}