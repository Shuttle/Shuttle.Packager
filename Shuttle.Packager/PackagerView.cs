using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Shuttle.Packager
{
    public partial class PackagerView : Form
    {
        private readonly Regex _assemblyVersionExpression =
            new Regex(@"AssemblyVersion\s*\(\s*""(?<version>.*)""\s*\)", RegexOptions.IgnoreCase);

        private readonly Regex _packageVersionExpression =
            new Regex(
                @"<PackageReference\s*Include=""(?<package>.*?)""\s*Version=""(?<version>(?<major>\d*)\.(?<minor>\d*)\.(?<patch>\d*))""\s*/>",
                RegexOptions.IgnoreCase);

        public PackagerView()
        {
            InitializeComponent();

            FetchPackages(Folder.Text);

            UpdateUsagesMenuItem.Click += UpdateUsages;
            FindUsagesMenuItem.Click += FindUsages;
            ShowLogMenuItem.Click += ShowLog;
            OpenMenuItem.Click += Open;

            var doubleBuffered =
                typeof(Control).GetProperty(
                    "DoubleBuffered",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

            if (doubleBuffered != null)
            {
                doubleBuffered.SetValue(BuildLog, true, null);
            }
        }

        private void FindUsages(object sender, EventArgs e)
        {
            if (Packages.FocusedItem == null)
            {
                return;
            }

            foreach (var dependentPackageMatch in FindUsages(Packages.FocusedItem.Package()))
            {
                dependentPackageMatch.Package.Checked = true;
            }
        }

        private void Open(object sender, EventArgs e)
        {
            Packages.FocusedItem?.Package().OpenSolution();
        }

        private void ShowLog(object sender, EventArgs e)
        {
            if (Packages.FocusedItem == null)
            {
                return;
            }

            var package = Packages.FocusedItem.Package();

            BuildLog.SelectionLength = 0;
            BuildLog.Text = package.BuildLog;
            PackageTabs.SelectTab(BuildLogTab);
            BuildLogTab.Text = package.Name;
        }

        private IEnumerable<DependentPackageMatch> FindUsages(Package package)
        {
            var result = new List<DependentPackageMatch>();

            foreach (ListViewItem item in Packages.Items)
            {
                var dependentPackage = item.Package();
                var content = File.ReadAllText(dependentPackage.ProjectPath);
                var matches = _packageVersionExpression.Matches(content);

                if (matches.Count == 0)
                {
                    continue;
                }

                foreach (Match match in matches)
                {
                    if (!match.Groups["package"].Value.Equals(package.Name,
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    result.Add(new DependentPackageMatch(item.Package(), match, content));

                    break;
                }
            }

            return result;
        }

        private void UpdateUsages(object sender, EventArgs e)
        {
            if (Packages.FocusedItem == null)
            {
                return;
            }

            var package = Packages.FocusedItem.Package();
            var dependencies = FindUsages(package);

            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            var log = new StringBuilder();

            foreach (var dependentPackageMatch in dependencies)
            {
                if (dependentPackageMatch.Match.Groups["version"].Value.Equals(
                    package.CurrentVersion.Formatted(),
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                dependentPackageMatch.Package.Checked = true;

                var updatedContent = dependentPackageMatch.ProjectContent.Substring(0, dependentPackageMatch.Match.Index) + $@"<PackageReference Include=""{dependentPackageMatch.Package.Name}"" Version=""{dependentPackageMatch.Package.CurrentVersion.Formatted()}"" />" + dependentPackageMatch.ProjectContent.Substring(dependentPackageMatch.Match.Index + dependentPackageMatch.Match.Length);

                File.WriteAllText(dependentPackageMatch.Package.ProjectPath, updatedContent);

                log.AppendLine(dependentPackageMatch.Package.Name);
            }

            if (log.Length > 0)
            {
                File.WriteAllText(
                    Path.Combine(logFolder, $"{Packages.FocusedItem.Name}-{package.CurrentVersion.Formatted()}-usages.log"),
                    log.ToString());
            }
            else
            {
                MessageBox.Show(@"No usages found that require an update.");
            }
        }

        private ListViewItem FindItem(string name)
        {
            return Packages.Items.Cast<ListViewItem>().FirstOrDefault(item =>
                item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private void FetchPackages(string folder)
        {
            Packages.Items.Clear();
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
                var packageName = Path.GetFileName(directory);
                var msbuildPath = Path.Combine(directory, ".build\\package.msbuild");
                var assemblyInfoPath = Path.Combine(directory, "Properties\\AssemblyInfo.cs");
                var projectPath = Path.Combine(directory, $"{packageName}.csproj");

                try
                {
                    if (!File.Exists(msbuildPath) 
                        || 
                        !File.Exists(assemblyInfoPath) 
                        || 
                        !File.Exists(projectPath))
                    {
                        continue;
                    }

                    var match = _assemblyVersionExpression.Match(File.ReadAllText(assemblyInfoPath));

                    if (!match.Success)
                    {
                        continue;
                    }

                    var item = Packages.Items.Add(packageName, packageName, "package");

                    item.SubItems.Add("-");
                    item.SubItems.Add(directory.Substring(root.Length + 1));

                    item.Tag = new Package(item, projectPath, msbuildPath,
                        new SemanticVersion(match.Groups["version"].Value));
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
            Build("build");
        }

        private void Build(string target)
        {
            BuildButton.Enabled = false;
            PackageButton.Enabled = false;
            ReleaseButton.Enabled = false;

            foreach (ListViewItem item in Packages.CheckedItems)
            {
                item.ImageKey = @"hourglass";

                Execute(item.Package(), target);

                if (!target.Equals("build", StringComparison.InvariantCultureIgnoreCase))
                {
                    item.Package().ApplyBuildVersion();
                }

                if (item.Package().HasFailed())
                {
                    item.ImageKey = @"cross";
                }
                else
                {
                    item.ImageKey = @"tick";
                    item.Checked = false;
                }
            }

            BuildButton.Enabled = true;
            PackageButton.Enabled = true;
            ReleaseButton.Enabled = true;
        }

        private void Execute(Package package, string target)
        {
            BuildLog.Text = string.Empty;
            PackageTabs.SelectTab(BuildLogTab);
            BuildLogTab.Text = package.Name;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(package.MSBuildPath),
                    Arguments = Path.GetFileName(package.MSBuildPath) +
                                $" /p:SemanticVersion={package.BuildVersion.Formatted()}" +
                                (!string.IsNullOrEmpty(target) ? $" /t:{target}" : string.Empty),
                    FileName =
                        @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, args) =>
            {
                BeginInvoke(new Action(() =>
                {
                    BuildLog.SelectionStart = BuildLog.TextLength;
                    BuildLog.SelectedText = args.Data + Environment.NewLine;
                    BuildLog.Refresh();
                }));
            };

            process.Start();
            process.BeginOutputReadLine();

            while (!process.HasExited)
            {
                Application.DoEvents();
            }

            process.CancelOutputRead();

            package.CaptureBuildLog(BuildLog.Text);
        }

        private void PackageButton_Click(object sender, EventArgs e)
        {
            Build("package");
        }

        private void ReleaseButton_Click(object sender, EventArgs e)
        {
            Build(string.Empty);
        }

        private void Packages_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Packages.FocusedItem.Bounds.Contains(e.Location))
                {
                    PackageContextMenu.Show(Cursor.Position);
                }
            }
        }

        private void Packages_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Packages.FocusedItem.Bounds.Contains(e.Location))
            {
                return;
            }

            Packages.FocusedItem.Package().OpenSolution();
        }
    }
}