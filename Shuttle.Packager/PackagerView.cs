using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace Shuttle.Packager;

public partial class PackagerView : Form
{
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly string _msbuildPath;

    private readonly Regex _packageVersionExpression = new(@"<PackageReference\s*Include=""(?<package>.*?)""\s*Version=""(?<version>(?<major>\d*)\.(?<minor>\d*)\.(?<patch>\d*))""\s*/>", RegexOptions.IgnoreCase);

    private readonly List<string> _skipFolders = new()
    {
        "node_modules",
        "src"
    };

    private readonly Regex _versionExpression = new(@"\<version\>(?<version>.*)</version\>", RegexOptions.IgnoreCase);
    private readonly string _visualStudioPath;

    public PackagerView(IConfiguration configuration)
    {
        InitializeComponent();

        _cancellationToken = _cancellationTokenSource.Token;

        Load += (_, _) =>
        {
            foreach (var file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images")))
            {
                ImageList.Images.Add(Path.GetFileNameWithoutExtension(file), Image.FromFile(file));
            }

            FetchPackages(Folder.Text);
        };

        UpdateUsagesMenuItem.Click += UpdateUsages;
        MarkUsagesMenuItem.Click += (_, _) =>
        {
            FindUsages(true);
        };
        ShowUsagesMenuItem.Click += (_, _) =>
        {
            FindUsages(false);
        };
        ShowLogMenuItem.Click += ShowLog;
        OpenMenuItem.Click += Open;
        GitHubMenuItem.Click += GitHub;
        RemoveFromNugetCacheMenuItem.Click += RemoveFromNugetCache;

        _msbuildPath = configuration["MSBuildPath"] ?? throw new ApplicationException("Could not find the `MSBuildPath` configuration setting.");

        if (!_msbuildPath.ToLower().EndsWith("msbuild.exe"))
        {
            _msbuildPath = Path.Combine(_msbuildPath, "MSBuild.exe");
        }

        _visualStudioPath = configuration["VisualStudioPath"] ?? throw new ApplicationException("Could not find the `VisualStudioPath` configuration setting.");

        if (!_visualStudioPath.ToLower().EndsWith("devenv.exe"))
        {
            _visualStudioPath = Path.Combine(_visualStudioPath, "devenv.exe");
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
                    RemoveFromNugetCache(item.GetPackage());
                }

                item.ImageKey = @"hourglass";

                Execute(item.GetPackage(), target);

                if (!target.Equals("build", StringComparison.InvariantCultureIgnoreCase))
                {
                    item.GetPackage().ApplyBuildVersion();
                }

                if (item.GetPackage().HasFailed())
                {
                    item.ImageKey = @"cross";
                }
                else
                {
                    item.ImageKey = @"tick";
                    item.Checked = false;

                    if (DebugNugetPackage.Checked)
                    {
                        ApplyDebugNugetPackage(item.GetPackage());
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

    private void BuildButton_Click(object sender, EventArgs e)
    {
        Build("build");
    }

    private void button1_Click(object sender, EventArgs e)
    {
        foreach (var item in CheckedPackages())
        {
            item.GetPackage().SetPrerelease(Prerelease.Text);
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

    private void ClearButton_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem item in Packages.Items)
        {
            item.Checked = false;
        }
    }

    private void ClearUsages()
    {
        UsageColumn.Text = @"Usages";

        foreach (ListViewItem item in Packages.Items)
        {
            var subItem = item.SubItems["Usage"];

            if (subItem == null)
            {
                continue;
            }

            subItem.Text = string.Empty;
        }
    }

    private void ClearUsagesButton_Click(object sender, EventArgs e)
    {
        ClearUsages();
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
                WorkingDirectory = Path.GetDirectoryName(package.MSBuildPath) ?? throw new ApplicationException($"Could not get the directory name from path '{package.MSBuildPath}'."),
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

        process.OutputDataReceived += (_, args) =>
        {
            if (IsDisposed)
            {
                return;
            }

            BeginInvoke(() =>
            {
                LogMessage(args.Data ?? string.Empty);
            });
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

    private void FetchButton_Click(object sender, EventArgs e)
    {
        FetchPackages(Folder.Text);
    }

    private void FetchPackages(string folder)
    {
        Packages.Items.Clear();

        FetchButton.Enabled = false;

        Task.Run(() =>
        {
            FetchPackages(folder, folder);

            Invoke(() =>
            {
                FetchButton.Enabled = true;
            });
        }, _cancellationToken);
    }

    private void FetchPackages(string folder, string root)
    {
        if (!Directory.Exists(folder) || ShouldSkipFolder(folder))
        {
            return;
        }

        foreach (var directory in Directory.GetDirectories(folder))
        {
            if (ShouldSkipFolder(directory))
            {
                continue;
            }

            var packageName = Path.GetFileName(directory);
            var projectPath = Path.Combine(directory, $"{packageName}.csproj");
            var nuspecPath = Path.Combine(directory, ".package\\package.nuspec");
            var msbuildPath = Path.Combine(directory, ".package\\package.msbuild");

            try
            {
                if (!File.Exists(msbuildPath)
                    ||
                    !File.Exists(nuspecPath)
                    ||
                    !File.Exists(projectPath))
                {
                    continue;
                }

                var match = _versionExpression.Match(File.ReadAllText(nuspecPath));

                if (!match.Success)
                {
                    continue;
                }

                Invoke(() =>
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

        Invoke(() =>
        {
            PackageNameColumn.Width = -1;
            LocationColumn.Width = -1;
        });
    }

    private void FindUsages(bool mark)
    {
        if (Packages.FocusedItem == null)
        {
            return;
        }

        foreach (var dependentPackageMatch in FindUsages(Packages.FocusedItem.GetPackage()))
        {
            if (mark)
            {
                dependentPackageMatch.Package.Checked = true;
            }

            dependentPackageMatch.ShowUsage();
        }
    }

    private IEnumerable<DependentPackageMatch> FindUsages(Package package)
    {
        UsageColumn.Text = package.Name;
        UsageColumn.Width = -2;

        var result = new List<DependentPackageMatch>();

        foreach (ListViewItem item in Packages.Items)
        {
            var dependentPackage = item.GetPackage();
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

                result.Add(new DependentPackageMatch(item.GetPackage(), match, content));

                break;
            }
        }

        return result;
    }

    private void FolderButton_Click(object sender, EventArgs e)
    {
        if (FolderBrowser.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        Folder.Text = FolderBrowser.SelectedPath;
    }

    private void GitHub(object? sender, EventArgs e)
    {
        if (Packages.FocusedItem == null)
        {
            return;
        }

        Process.Start($"https://github.com/Shuttle/{Packages.FocusedItem.GetPackage().Name}");
    }

    private void InvertButton_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem item in Packages.Items)
        {
            item.Checked = !item.Checked;
        }
    }

    private void LogMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        BuildLog.SelectionStart = BuildLog.TextLength;
        BuildLog.SelectedText = message + Environment.NewLine;
        BuildLog.Refresh();
    }

    private void MajorButton_Click(object sender, EventArgs e)
    {
        foreach (var item in CheckedPackages())
        {
            item.GetPackage().IncreaseMajor();
        }
    }

    private void MinorButton_Click(object sender, EventArgs e)
    {
        foreach (var item in CheckedPackages())
        {
            item.GetPackage().IncreaseMinor();
        }
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
            var package = packagesItem.Tag as Package;

            if (package == null || package.NugetVersion != null)
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

                    Invoke(() =>
                    {
                        package.ApplyNugetVersion(
                            new SemanticVersion(string.IsNullOrWhiteSpace(version) ? "0.0.0" : version));
                    });
                }
            }

            Invoke(() =>
            {
                NuGetVersionsButton.Enabled = true;
            });
        }, _cancellationToken);
    }

    private void Open(object? sender, EventArgs e)
    {
        Packages.FocusedItem?.GetPackage().OpenSolution(_visualStudioPath);
    }

    private void PackageButton_Click(object sender, EventArgs e)
    {
        Build("bump");
    }

    private void PackagerView_FormClosing(object sender, FormClosingEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }

    private void Packages_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right || Packages.FocusedItem == null)
        {
            return;
        }

        if (Packages.FocusedItem.Bounds.Contains(e.Location))
        {
            PackageContextMenu.Show(Cursor.Position);
        }
    }

    private void Packages_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (Packages.FocusedItem == null || !Packages.FocusedItem.Bounds.Contains(e.Location))
        {
            return;
        }

        Packages.FocusedItem.GetPackage().OpenSolution(_visualStudioPath);
    }

    private void PatchButton_Click(object sender, EventArgs e)
    {
        foreach (var item in CheckedPackages())
        {
            item.GetPackage().IncreasePatch();
        }
    }

    private void ReleaseButton_Click(object sender, EventArgs e)
    {
        Build("push");
    }

    private void RemoveFromNugetCache(object? sender, EventArgs e)
    {
        if (Packages.FocusedItem == null)
        {
            return;
        }

        RemoveFromNugetCache(Packages.FocusedItem.GetPackage());
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

    private void ResetButton_Click(object sender, EventArgs e)
    {
        foreach (var item in CheckedPackages())
        {
            item.GetPackage().ResetVersion();
        }
    }

    private void Restore(Package package)
    {
        LogMessage("[restore]");
        LogMessage("");


        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetDirectoryName(package.ProjectPath) ?? throw new ApplicationException($"Could not get directory name from path '{package.ProjectPath}'."),
                Arguments = "restore " + Path.GetFileName(package.ProjectPath),
                FileName = @"C:\Program Files\dotnet\dotnet.exe",
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += (_, args) =>
        {
            BeginInvoke(() =>
            {
                LogMessage(args.Data ?? string.Empty);
            });
        };

        process.Start();
        process.BeginOutputReadLine();

        while (!process.HasExited)
        {
            Application.DoEvents();
        }

        process.CancelOutputRead();
    }

    private bool ShouldSkipFolder(string folder)
    {
        var name = Path.GetFileName(folder);

        return name.StartsWith(".") || _skipFolders.Contains(name);
    }

    private void ShowLog(object? sender, EventArgs e)
    {
        if (Packages.FocusedItem == null)
        {
            return;
        }

        var package = Packages.FocusedItem.GetPackage();

        BuildLog.SelectionLength = 0;
        BuildLog.Text = package.BuildLog;
        PackageTabs.SelectTab(BuildLogTab);
        BuildLogTab.Text = package.Name;
    }

    private void UpdateUsages(object? sender, EventArgs e)
    {
        if (Packages.FocusedItem == null)
        {
            return;
        }

        var package = Packages.FocusedItem.GetPackage();
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
}