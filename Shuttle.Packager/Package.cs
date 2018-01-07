using System.Windows.Forms;

namespace Shuttle.Packager
{
    internal class Package
    {
        private readonly ListViewItem _item;

        public Package(ListViewItem item, string msbuildPath, SemanticVersion currentVersion)
        {
            _item = item;
            CurrentVersion = currentVersion;
            BuildVersion = CurrentVersion.Copy();
            MSBuildPath = msbuildPath;
        }

        public SemanticVersion CurrentVersion { get; }
        public SemanticVersion BuildVersion { get; private set; }
        public string MSBuildPath { get; }

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
    }
}