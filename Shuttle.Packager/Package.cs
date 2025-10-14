using System.Diagnostics;

namespace Shuttle.Packager;

public class Package
{
    private readonly ListViewItem _item;
    private string _solutionPath = string.Empty;

    public Package(ListViewItem item, string projectPath, string msbuildPath, SemanticVersion currentVersion)
    {
        _item = item;
        ProjectPath = projectPath;
        CurrentVersion = currentVersion;
        BuildVersion = CurrentVersion.Copy();
        MSBuildPath = msbuildPath;
        BuildLog = string.Empty;

        RenderVersion();
    }

    public string BuildLog { get; private set; }
    public SemanticVersion BuildVersion { get; private set; }

    public bool Checked
    {
        get => _item.Checked;
        set => _item.Checked = value;
    }

    public SemanticVersion CurrentVersion { get; private set; }
    public string MSBuildPath { get; }

    public string Name => _item.Text;
    public SemanticVersion? NugetVersion { get; private set; }
    public string ProjectPath { get; }

    public void ApplyBuildVersion()
    {
        CurrentVersion = BuildVersion.Copy();

        RenderVersion();
    }

    public void ApplyNugetVersion(SemanticVersion version)
    {
        NugetVersion = version;

        var subItem = _item.SubItems["NuGetVersion"];

        if (subItem == null)
        {
            return;
        }

        var value = version.Formatted();

        if (!CurrentVersion.Formatted().Equals(value))
        {
            subItem.Font = new Font(subItem.Font.FontFamily, subItem.Font.Size, FontStyle.Bold);
            subItem.ForeColor = Color.DarkRed;
        }
        else
        {
            subItem.Font = _item.Font;
            subItem.ForeColor = _item.ForeColor;
        }

        subItem.Text = value ?? string.Empty;
    }

    public void CaptureBuildLog(string text)
    {
        BuildLog = text;
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

    public bool HasFailed()
    {
        return !BuildLog.ToLower().Contains("build succeeded.");
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

    public void OpenSolution(string visualStudioPath)
    {
        if (string.IsNullOrEmpty(GetSolutionPath()))
        {
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = visualStudioPath,
            Arguments = GetSolutionPath()
        };

        Process.Start(startInfo);
    }

    private void RenderVersion()
    {
        var subItem = _item.SubItems["Version"];

        if (subItem == null)
        {
            return;
        }

        subItem.Text = CurrentVersion.IsEqualTo(BuildVersion)
            ? CurrentVersion.Formatted()
            : CurrentVersion.Formatted() + " => " + BuildVersion.Formatted();
    }

    public void ResetVersion()
    {
        BuildVersion = CurrentVersion.Copy();

        RenderVersion();
    }

    public Package SetPrerelease(string prerelease)
    {
        BuildVersion.SetPrerelease(prerelease);

        RenderVersion();

        return this;
    }

    public void ShowUsage(string version)
    {
        var subItem = _item.SubItems["Usage"];

        if (subItem == null)
        {
            return;
        }

        subItem.Text = version;
    }
}