using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shuttle.Core.Configuration;

namespace Shuttle.Packager
{
    public partial class PackagerView : Form
    {
        private readonly Regex _assemblyVersionExpression =
            new Regex(@"AssemblyVersion\s*\(\s*""(?<version>.*)""\s*\)", RegexOptions.IgnoreCase);

        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly string _msbuildPath;

        private readonly Regex _packageVersionExpression =
            new Regex(
                @"<PackageReference\s*Include=""(?<package>.*?)""\s*Version=""(?<version>(?<major>\d*)\.(?<minor>\d*)\.(?<patch>\d*))""\s*/>",
                RegexOptions.IgnoreCase);

        private readonly List<string> _skipFolders = new List<string>
        {
            ".git",
            "node_modules"
        };

        public PackagerView()
        {
            InitializeComponent();

            _cancellationToken = _cancellationTokenSource.Token;

            FetchPackages(Folder.Text);

            UpdateUsagesMenuItem.Click += UpdateUsages;
            MarkUsagesMenuItem.Click += (sender, e) =>
            {
                FindUsages(true);
            };
            ShowUsagesMenuItem.Click += (sender, e) =>
            {
                FindUsages(false);
            };
            ShowLogMenuItem.Click += ShowLog;
            OpenMenuItem.Click += Open;
            GitHubMenuItem.Click += GitHub;
            RemoveFromNugetCacheMenuItem.Click += RemoveFromNugetCache;

            _msbuildPath = ConfigurationItem<string>.ReadSetting("MSBuildPath").GetValue();

            if (!_msbuildPath.ToLower().EndsWith("msbuild.exe"))
            {
                _msbuildPath = Path.Combine(_msbuildPath, "MSBuild.exe");
            }

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

        private void RemoveFromNugetCache(object sender, EventArgs e)
        {
            if (Packages.FocusedItem == null)
            {
                return;
            }

            RemoveFromNugetCache(Packages.FocusedItem.Package());
        }

        private void RemoveFromNugetCache(Package package)
        {
            try
            {
                var path =
                    $@"{Environment.ExpandEnvironmentVariables("%UserProfile%")}\.nuget\packages\{package.Name}";

                if (!Directory.Exists(path))
                {
                    return;
                }

                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Remove from Nuget cache", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FindUsages(bool mark)
        {
            if (Packages.FocusedItem == null)
            {
                return;
            }

            foreach (var dependentPackageMatch in FindUsages(Packages.FocusedItem.Package()))
            {
                if (mark)
                {
                    dependentPackageMatch.Package.Checked = true;
                }

                dependentPackageMatch.ShowUsage();
            }
        }

        private void Open(object sender, EventArgs e)
        {
            Packages.FocusedItem?.Package().OpenSolution();
        }

        private void GitHub(object sender, EventArgs e)
        {
            if (Packages.FocusedItem == null)
            {
                return;
            }

            Process.Start($"https://github.com/Shuttle/{Packages.FocusedItem.Package().Name}");
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
            UsageColumn.Text = package.Name;
            UsageColumn.Width = -2;

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
                if (dependentPackageMatch.GetVersion().Equals(
                        package.CurrentVersion.Formatted(),
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                dependentPackageMatch.Package.Checked = true;
                dependentPackageMatch.ShowUsage();

                var updatedContent =
                    dependentPackageMatch.ProjectContent.Substring(0, dependentPackageMatch.Match.Index) +
                    $@"<PackageReference Include=""{package.Name}"" Version=""{
                        package.CurrentVersion.Formatted()
                    }"" />" + dependentPackageMatch.ProjectContent.Substring(
                        dependentPackageMatch.Match.Index + dependentPackageMatch.Match.Length);

                File.WriteAllText(dependentPackageMatch.Package.ProjectPath, updatedContent);

                log.AppendLine(dependentPackageMatch.Package.Name);
            }

            if (log.Length > 0)
            {
                File.WriteAllText(
                    Path.Combine(logFolder,
                        $"{Packages.FocusedItem.Name}-{package.CurrentVersion.Formatted()}-usages.log"),
                    log.ToString());
            }
            else
            {
                MessageBox.Show(@"No usages found that require an update.");
            }
        }

        private void FetchPackages(string folder)
        {
            Packages.Items.Clear();

            FetchButton.Enabled = false;

            Task.Run(() =>
            {
                FetchPackages(folder, folder);

                this.Invoke(() =>
                {
                    FetchButton.Enabled = true;
                });
            });
        }

        private void FetchPackages(string folder, string root)
        {
            var name = Path.GetFileName(folder);

            if (!Directory.Exists(folder) ||
                _skipFolders.Contains(name))
            {
                return;
            }

            foreach (var directory in Directory.GetDirectories(folder))
            {
                var packageName = Path.GetFileName(directory);
                var assemblyInfoPath = Path.Combine(directory, "Properties\\AssemblyInfo.cs");
                var projectPath = Path.Combine(directory, $"{packageName}.csproj");
                var msbuildPath = Path.Combine(directory, ".package\\package.msbuild");

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

                    this.Invoke(() =>
                    {
                        var item = Packages.Items.Add(packageName, packageName, "package");

                        item.UseItemStyleForSubItems = false;
                        item.SubItems.Add("-").Name = @"Version";
                        item.SubItems.Add("-").Name = @"NuGetVersion";
                        item.SubItems.Add(string.Empty).Name = @"Usage";
                        item.SubItems.Add(directory.Substring(root.Length + 1)).Name = @"Location";

                        item.Tag = new Package(item, projectPath, msbuildPath,
                            new SemanticVersion(match.Groups["version"].Value));
                    });
                }
                finally
                {
                    FetchPackages(directory, root);
                }
            }

            this.Invoke(() =>
            {
                PackageNameColumn.Width = -1;
                LocationColumn.Width = -1;
            });
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
            foreach (var item in CheckedPackages())
            {
                item.Package().IncreaseMajor();
            }
        }

        private IEnumerable<ListViewItem> CheckedPackages()
        {
            var result = new List<ListViewItem>();

            if (Packages.CheckedItems.Count == 0 && Packages.FocusedItem != null)
            {
                Packages.FocusedItem.Checked = true;
            }

            foreach (ListViewItem item in Packages.Items)
            {
                if (!item.Checked)
                {
                    continue;
                }

                result.Add(item);
            }

            return result;
        }

        private void MinorButton_Click(object sender, EventArgs e)
        {
            foreach (var item in CheckedPackages())
            {
                item.Package().IncreaseMinor();
            }
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            foreach (var item in CheckedPackages())
            {
                item.Package().IncreasePatch();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            foreach (var item in CheckedPackages())
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
            try
            {
                BuildButton.Enabled = false;
                PackageButton.Enabled = false;
                ReleaseButton.Enabled = false;

                foreach (var item in CheckedPackages())
                {
                    if (target == "bump" && !DebugNugetPackage.Checked)
                    {
                        RemoveFromNugetCache(item.Package());
                    }

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

                        if (DebugNugetPackage.Checked)
                        {
                            ApplyDebugNugetPackage(item.Package());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
            finally
            {
                BuildButton.Enabled = true;
                PackageButton.Enabled = true;
                ReleaseButton.Enabled = true;
            }
        }

        private void ApplyDebugNugetPackage(Package package)
        {
            var nugetPackageLibFolder = Environment.ExpandEnvironmentVariables(
                $"%UserProfile%\\.nuget\\packages\\{package.Name}\\{package.BuildVersion.Formatted()}\\lib");
            var debugFolder = $"{Path.GetDirectoryName(package.ProjectPath)}\\bin\\Debug";

            if (!Directory.Exists(nugetPackageLibFolder))
            {
                LogMessage($"[ERROR] : nuget package folder does not exist - {nugetPackageLibFolder}");
                return;
            }

            if (!Directory.Exists(debugFolder))
            {
                LogMessage($"[ERROR] : debug folder does not exist - {debugFolder}");
                return;
            }

            Copy(new DirectoryInfo(debugFolder), new DirectoryInfo(nugetPackageLibFolder));
        }

        public static void Copy(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (var fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                Copy(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void Execute(Package package, string target)
        {
            BuildLog.Text = string.Empty;
            PackageTabs.SelectTab(BuildLogTab);
            BuildLogTab.Text = package.Name;

            var deploymentFolder =
                Path.Combine(
                    Path.GetDirectoryName(package.MSBuildPath) ??
                    throw new InvalidOperationException("Could not get MSBuildPath directory name."), "deployment");

            if (Directory.Exists(deploymentFolder))
            {
                Directory.Delete(deploymentFolder, true);
            }

            var outputFolder =
                Path.Combine(
                    Path.GetDirectoryName(package.ProjectPath) ??
                    throw new InvalidOperationException("Could not get ProjectPath directory name."), "bin");

            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }

            Restore(package);

            LogMessage("[build]");
            LogMessage("");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(package.MSBuildPath),
                    Arguments = Path.GetFileName(package.MSBuildPath) +
                                $" /p:SemanticVersion={package.BuildVersion.Formatted()}" +
                                (!string.IsNullOrEmpty(target) ? $" /t:{target}" : string.Empty),
                    FileName = _msbuildPath,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, args) =>
            {
                if (IsDisposed)
                {
                    return;
                }

                BeginInvoke(new Action(() =>
                {
                    LogMessage(args.Data);
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

        private void LogMessage(string message)
        {
            BuildLog.SelectionStart = BuildLog.TextLength;
            BuildLog.SelectedText = message + Environment.NewLine;
            BuildLog.Refresh();
        }

        private void Restore(Package package)
        {
            LogMessage("[restore]");
            LogMessage("");


            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Path.GetDirectoryName(package.ProjectPath),
                    Arguments = "restore " + Path.GetFileName(package.ProjectPath),
                    FileName = @"C:\Program Files\dotnet\dotnet.exe",
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
                    LogMessage(args.Data);
                }));
            };

            process.Start();
            process.BeginOutputReadLine();

            while (!process.HasExited)
            {
                Application.DoEvents();
            }

            process.CancelOutputRead();
        }

        private void PackageButton_Click(object sender, EventArgs e)
        {
            Build("bump");
        }

        private void ReleaseButton_Click(object sender, EventArgs e)
        {
            Build("push");
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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.Items)
            {
                item.Checked = false;
            }
        }

        private void InvertButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in Packages.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void ClearUsagesButton_Click(object sender, EventArgs e)
        {
            ClearUsages();
        }

        private void ClearUsages()
        {
            UsageColumn.Text = @"Usages";

            foreach (ListViewItem item in Packages.Items)
            {
                item.SubItems["Usage"].Text = string.Empty;
            }
        }

        private void PackagerView_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void NuGetVersionsButton_Click(object sender, EventArgs e)
        {
            if (!FetchButton.Enabled)
            {
                return;
            }

            NuGetVersionsButton.Enabled = false;

            var packages = new List<Package>();

            foreach (ListViewItem packagesItem in Packages.Items)
            {
                var package = (Package)packagesItem.Tag;

                if (package.NugetVersion != null)
                {
                    continue;
                }

                packages.Add(package);
            }

            Task.Run(() =>
            {
                var httpClientHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = false
                };

                using (var client = new HttpClient(httpClientHandler))
                {
                    foreach (var package in packages)
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        var version = string.Empty;

                        using (var response = client
                                   .GetAsync(
                                       new Uri($"https://api.nuget.org/v3-flatcontainer/{package.Name}/index.json"),
                                       _cancellationToken).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                using (var document = JsonDocument.Parse(response.Content.ReadAsStringAsync().Result))
                                {
                                    version = document.RootElement.GetProperty("versions").EnumerateArray()
                                        .LastOrDefault()
                                        .ToString();
                                }
                            }
                        }

                        this.Invoke(() =>
                        {
                            package.ApplyNugetVersion(
                                new SemanticVersion(string.IsNullOrWhiteSpace(version) ? "0.0.0" : version));
                        });
                    }
                }

                this.Invoke(() =>
                {
                    NuGetVersionsButton.Enabled = true;
                });
            }, _cancellationToken);
        }
    }
}