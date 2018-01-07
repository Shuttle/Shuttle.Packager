using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Shuttle.Packager
{
    public partial class PackagerView : Form
    {
        private readonly Regex _assemblyVersionExpression =
            new Regex(@"AssemblyVersion\s*\(\s*""(?<version>.*)""\s*\)", RegexOptions.IgnoreCase);

        public PackagerView()
        {
            InitializeComponent();

            FetchPackages(Folder.Text);
        }

        private void FetchPackages(string folder)
        {
            FetchPackages(folder, folder);
        }

        private void FetchPackages(string folder, string root)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }

            foreach (var directory in Directory.GetDirectories(folder))
            {
                var msbuildPath = Path.Combine(directory, ".build\\package.msbuild");
                var assemblyInfoPath = Path.Combine(directory, "Properties\\AssemblyInfo.cs");

                try
                {
                    if (!File.Exists(msbuildPath) || !File.Exists(assemblyInfoPath))
                    {
                        continue;
                    }

                    var match = _assemblyVersionExpression.Match(File.ReadAllText(assemblyInfoPath));

                    if (!match.Success)
                    {
                        continue;
                    }

                    var item = Packages.Items.Add(Path.GetFileName(directory));

                    item.SubItems.Add(match.Groups["version"].Value);
                    item.SubItems.Add(directory.Substring(root.Length + 1));

                    item.Tag = new Package(item, msbuildPath, new SemanticVersion(match.Groups["version"].Value));
                }
                finally
                {
                    FetchPackages(directory, root);
                }
            }

            PackageNameColumn.Width = -1;
            LocationColumn.Width = -1;
        }

        private void FolderButton_Click(object sender, EventArgs e)
        {
            if (FolderBrowser.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Folder.Text = FolderBrowser.SelectedPath;
        }

        private void FetchButton_Click(object sender, EventArgs e)
        {
            FetchPackages(Folder.Text);
        }

        private void MajorButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.CheckedItems)
            {
                item.Package().IncreaseMajor();
            }
        }

        private void MinorButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.CheckedItems)
            {
                item.Package().IncreaseMinor();
            }
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.CheckedItems)
            {
                item.Package().IncreasePatch();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.CheckedItems)
            {
                item.Package().ResetVersion();
            }
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.CheckedItems)
            {
                Execute(item.Package(), "build");
            }
        }

        private void Execute(Package package, string target)
        {
            var psi = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(package.MSBuildPath),
                Arguments = Path.GetFileName(package.MSBuildPath) + $" /p:SemanticVersion={package.BuildVersion.Formatted()}" + (!string.IsNullOrEmpty(target) ? $" /t:{target}" : string.Empty),
                FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
            };

            Process.Start(psi);
        }
    }
}