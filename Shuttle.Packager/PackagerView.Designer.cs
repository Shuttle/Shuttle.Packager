namespace Shuttle.Packager
{
    partial class PackagerView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            Folder = new TextBox();
            FolderButton = new Button();
            FolderBrowser = new FolderBrowserDialog();
            FetchButton = new Button();
            BuildButton = new Button();
            MajorButton = new Button();
            MinorButton = new Button();
            PatchButton = new Button();
            ResetButton = new Button();
            PackageTabs = new TabControl();
            PackageTab = new TabPage();
            Packages = new ListView();
            PackageNameColumn = new ColumnHeader();
            VersionColumn = new ColumnHeader();
            NuGetVersionColumn = new ColumnHeader();
            UsageColumn = new ColumnHeader();
            LocationColumn = new ColumnHeader();
            ImageList = new ImageList(components);
            BuildLogTab = new TabPage();
            BuildLog = new TextBox();
            PackageButton = new Button();
            ReleaseButton = new Button();
            PackageContextMenu = new ContextMenuStrip(components);
            UpdateUsagesMenuItem = new ToolStripMenuItem();
            MarkUsagesMenuItem = new ToolStripMenuItem();
            ShowUsagesMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            OpenMenuItem = new ToolStripMenuItem();
            GitHubMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            RemoveFromNugetCacheMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            ShowLogMenuItem = new ToolStripMenuItem();
            InvertButton = new Button();
            ClearButton = new Button();
            ClearUsagesButton = new Button();
            NuGetVersionsButton = new Button();
            DebugNugetPackage = new CheckBox();
            Prerelease = new TextBox();
            label2 = new Label();
            button1 = new Button();
            PackageTabs.SuspendLayout();
            PackageTab.SuspendLayout();
            BuildLogTab.SuspendLayout();
            PackageContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 18);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(40, 15);
            label1.TabIndex = 0;
            label1.Text = "Folder";
            // 
            // Folder
            // 
            Folder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Folder.Location = new Point(18, 37);
            Folder.Margin = new Padding(2, 3, 2, 3);
            Folder.Name = "Folder";
            Folder.Size = new Size(1138, 23);
            Folder.TabIndex = 1;
            Folder.Text = "D:\\development.github\\shuttle";
            // 
            // FolderButton
            // 
            FolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FolderButton.Location = new Point(1069, 75);
            FolderButton.Margin = new Padding(2, 3, 2, 3);
            FolderButton.Name = "FolderButton";
            FolderButton.Size = new Size(88, 37);
            FolderButton.TabIndex = 7;
            FolderButton.Text = "Select";
            FolderButton.UseVisualStyleBackColor = true;
            FolderButton.Click += FolderButton_Click;
            // 
            // FetchButton
            // 
            FetchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FetchButton.Location = new Point(964, 75);
            FetchButton.Margin = new Padding(2, 3, 2, 3);
            FetchButton.Name = "FetchButton";
            FetchButton.Size = new Size(88, 37);
            FetchButton.TabIndex = 6;
            FetchButton.Text = "Fetch";
            FetchButton.UseVisualStyleBackColor = true;
            FetchButton.Click += FetchButton_Click;
            // 
            // BuildButton
            // 
            BuildButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BuildButton.Location = new Point(1066, 786);
            BuildButton.Margin = new Padding(2, 3, 2, 3);
            BuildButton.Name = "BuildButton";
            BuildButton.Size = new Size(88, 42);
            BuildButton.TabIndex = 19;
            BuildButton.Text = "Build";
            BuildButton.UseVisualStyleBackColor = true;
            BuildButton.Click += BuildButton_Click;
            // 
            // MajorButton
            // 
            MajorButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MajorButton.Location = new Point(18, 786);
            MajorButton.Margin = new Padding(2, 3, 2, 3);
            MajorButton.Name = "MajorButton";
            MajorButton.Size = new Size(52, 42);
            MajorButton.TabIndex = 9;
            MajorButton.Text = "Major";
            MajorButton.UseVisualStyleBackColor = true;
            MajorButton.Click += MajorButton_Click;
            // 
            // MinorButton
            // 
            MinorButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MinorButton.Location = new Point(70, 786);
            MinorButton.Margin = new Padding(2, 3, 2, 3);
            MinorButton.Name = "MinorButton";
            MinorButton.Size = new Size(52, 42);
            MinorButton.TabIndex = 10;
            MinorButton.Text = "Minor";
            MinorButton.UseVisualStyleBackColor = true;
            MinorButton.Click += MinorButton_Click;
            // 
            // PatchButton
            // 
            PatchButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PatchButton.Location = new Point(122, 786);
            PatchButton.Margin = new Padding(2, 3, 2, 3);
            PatchButton.Name = "PatchButton";
            PatchButton.Size = new Size(52, 42);
            PatchButton.TabIndex = 11;
            PatchButton.Text = "Patch";
            PatchButton.UseVisualStyleBackColor = true;
            PatchButton.Click += PatchButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ResetButton.Location = new Point(450, 786);
            ResetButton.Margin = new Padding(2, 3, 2, 3);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(52, 42);
            ResetButton.TabIndex = 15;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // PackageTabs
            // 
            PackageTabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PackageTabs.Controls.Add(PackageTab);
            PackageTabs.Controls.Add(BuildLogTab);
            PackageTabs.Location = new Point(18, 132);
            PackageTabs.Margin = new Padding(2, 3, 2, 3);
            PackageTabs.Name = "PackageTabs";
            PackageTabs.SelectedIndex = 0;
            PackageTabs.Size = new Size(1136, 636);
            PackageTabs.TabIndex = 8;
            // 
            // PackageTab
            // 
            PackageTab.Controls.Add(Packages);
            PackageTab.Location = new Point(4, 24);
            PackageTab.Margin = new Padding(2, 3, 2, 3);
            PackageTab.Name = "PackageTab";
            PackageTab.Padding = new Padding(2, 3, 2, 3);
            PackageTab.Size = new Size(1128, 608);
            PackageTab.TabIndex = 0;
            PackageTab.Text = "Packages";
            PackageTab.UseVisualStyleBackColor = true;
            // 
            // Packages
            // 
            Packages.CheckBoxes = true;
            Packages.Columns.AddRange(new ColumnHeader[] { PackageNameColumn, VersionColumn, NuGetVersionColumn, UsageColumn, LocationColumn });
            Packages.Dock = DockStyle.Fill;
            Packages.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Packages.FullRowSelect = true;
            Packages.Location = new Point(2, 3);
            Packages.Margin = new Padding(2, 3, 2, 3);
            Packages.Name = "Packages";
            Packages.Size = new Size(1124, 602);
            Packages.SmallImageList = ImageList;
            Packages.TabIndex = 0;
            Packages.UseCompatibleStateImageBehavior = false;
            Packages.View = View.Details;
            Packages.MouseClick += Packages_MouseClick;
            Packages.MouseDoubleClick += Packages_MouseDoubleClick;
            // 
            // PackageNameColumn
            // 
            PackageNameColumn.Text = "Package";
            PackageNameColumn.Width = 200;
            // 
            // VersionColumn
            // 
            VersionColumn.Text = "Version";
            VersionColumn.TextAlign = HorizontalAlignment.Right;
            VersionColumn.Width = 100;
            // 
            // NuGetVersionColumn
            // 
            NuGetVersionColumn.Text = "NuGet Version";
            NuGetVersionColumn.TextAlign = HorizontalAlignment.Right;
            NuGetVersionColumn.Width = 120;
            // 
            // UsageColumn
            // 
            UsageColumn.Text = "Usage";
            UsageColumn.Width = 75;
            // 
            // LocationColumn
            // 
            LocationColumn.Text = "Location";
            LocationColumn.Width = 200;
            // 
            // ImageList
            // 
            ImageList.ColorDepth = ColorDepth.Depth8Bit;
            ImageList.ImageSize = new Size(16, 16);
            ImageList.TransparentColor = Color.Transparent;
            // 
            // BuildLogTab
            // 
            BuildLogTab.Controls.Add(BuildLog);
            BuildLogTab.Location = new Point(4, 24);
            BuildLogTab.Margin = new Padding(2, 3, 2, 3);
            BuildLogTab.Name = "BuildLogTab";
            BuildLogTab.Padding = new Padding(2, 3, 2, 3);
            BuildLogTab.Size = new Size(1128, 608);
            BuildLogTab.TabIndex = 1;
            BuildLogTab.Text = "Log";
            BuildLogTab.UseVisualStyleBackColor = true;
            // 
            // BuildLog
            // 
            BuildLog.BackColor = Color.Black;
            BuildLog.Dock = DockStyle.Fill;
            BuildLog.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BuildLog.ForeColor = Color.FromArgb(224, 224, 224);
            BuildLog.Location = new Point(2, 3);
            BuildLog.Margin = new Padding(2, 3, 2, 3);
            BuildLog.Multiline = true;
            BuildLog.Name = "BuildLog";
            BuildLog.ReadOnly = true;
            BuildLog.ScrollBars = ScrollBars.Both;
            BuildLog.Size = new Size(1124, 602);
            BuildLog.TabIndex = 0;
            // 
            // PackageButton
            // 
            PackageButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            PackageButton.Location = new Point(961, 786);
            PackageButton.Margin = new Padding(2, 3, 2, 3);
            PackageButton.Name = "PackageButton";
            PackageButton.Size = new Size(88, 42);
            PackageButton.TabIndex = 18;
            PackageButton.Text = "Package";
            PackageButton.UseVisualStyleBackColor = true;
            PackageButton.Click += PackageButton_Click;
            // 
            // ReleaseButton
            // 
            ReleaseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ReleaseButton.Location = new Point(856, 786);
            ReleaseButton.Margin = new Padding(2, 3, 2, 3);
            ReleaseButton.Name = "ReleaseButton";
            ReleaseButton.Size = new Size(88, 42);
            ReleaseButton.TabIndex = 17;
            ReleaseButton.Text = "Release";
            ReleaseButton.UseVisualStyleBackColor = true;
            ReleaseButton.Click += ReleaseButton_Click;
            // 
            // PackageContextMenu
            // 
            PackageContextMenu.ImageScalingSize = new Size(20, 20);
            PackageContextMenu.Items.AddRange(new ToolStripItem[] { UpdateUsagesMenuItem, MarkUsagesMenuItem, ShowUsagesMenuItem, toolStripSeparator2, OpenMenuItem, GitHubMenuItem, toolStripSeparator1, RemoveFromNugetCacheMenuItem, toolStripSeparator3, ShowLogMenuItem });
            PackageContextMenu.Name = "ItemContextMenu";
            PackageContextMenu.Size = new Size(217, 176);
            // 
            // UpdateUsagesMenuItem
            // 
            UpdateUsagesMenuItem.Name = "UpdateUsagesMenuItem";
            UpdateUsagesMenuItem.Size = new Size(216, 22);
            UpdateUsagesMenuItem.Text = "&Update usages";
            // 
            // MarkUsagesMenuItem
            // 
            MarkUsagesMenuItem.Name = "MarkUsagesMenuItem";
            MarkUsagesMenuItem.Size = new Size(216, 22);
            MarkUsagesMenuItem.Text = "&Mark usages";
            // 
            // ShowUsagesMenuItem
            // 
            ShowUsagesMenuItem.Name = "ShowUsagesMenuItem";
            ShowUsagesMenuItem.Size = new Size(216, 22);
            ShowUsagesMenuItem.Text = "&Show usages";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(213, 6);
            // 
            // OpenMenuItem
            // 
            OpenMenuItem.Name = "OpenMenuItem";
            OpenMenuItem.Size = new Size(216, 22);
            OpenMenuItem.Text = "&Open";
            // 
            // GitHubMenuItem
            // 
            GitHubMenuItem.Name = "GitHubMenuItem";
            GitHubMenuItem.Size = new Size(216, 22);
            GitHubMenuItem.Text = "GitHub";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(213, 6);
            // 
            // RemoveFromNugetCacheMenuItem
            // 
            RemoveFromNugetCacheMenuItem.Name = "RemoveFromNugetCacheMenuItem";
            RemoveFromNugetCacheMenuItem.Size = new Size(216, 22);
            RemoveFromNugetCacheMenuItem.Text = "&Remove from Nuget cache";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(213, 6);
            // 
            // ShowLogMenuItem
            // 
            ShowLogMenuItem.Name = "ShowLogMenuItem";
            ShowLogMenuItem.Size = new Size(216, 22);
            ShowLogMenuItem.Text = "Show &log";
            // 
            // InvertButton
            // 
            InvertButton.Location = new Point(18, 75);
            InvertButton.Margin = new Padding(2, 3, 2, 3);
            InvertButton.Name = "InvertButton";
            InvertButton.Size = new Size(88, 37);
            InvertButton.TabIndex = 2;
            InvertButton.Text = "Invert";
            InvertButton.UseVisualStyleBackColor = true;
            InvertButton.Click += InvertButton_Click;
            // 
            // ClearButton
            // 
            ClearButton.Location = new Point(122, 75);
            ClearButton.Margin = new Padding(2, 3, 2, 3);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(88, 37);
            ClearButton.TabIndex = 3;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // ClearUsagesButton
            // 
            ClearUsagesButton.Location = new Point(227, 75);
            ClearUsagesButton.Margin = new Padding(2, 3, 2, 3);
            ClearUsagesButton.Name = "ClearUsagesButton";
            ClearUsagesButton.Size = new Size(105, 37);
            ClearUsagesButton.TabIndex = 4;
            ClearUsagesButton.Text = "Clear usages";
            ClearUsagesButton.UseVisualStyleBackColor = true;
            ClearUsagesButton.Click += ClearUsagesButton_Click;
            // 
            // NuGetVersionsButton
            // 
            NuGetVersionsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            NuGetVersionsButton.Location = new Point(826, 75);
            NuGetVersionsButton.Margin = new Padding(2, 3, 2, 3);
            NuGetVersionsButton.Name = "NuGetVersionsButton";
            NuGetVersionsButton.Size = new Size(118, 37);
            NuGetVersionsButton.TabIndex = 5;
            NuGetVersionsButton.Text = "NuGet Versions";
            NuGetVersionsButton.UseVisualStyleBackColor = true;
            NuGetVersionsButton.Click += NuGetVersionsButton_Click;
            // 
            // DebugNugetPackage
            // 
            DebugNugetPackage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            DebugNugetPackage.AutoSize = true;
            DebugNugetPackage.Location = new Point(700, 799);
            DebugNugetPackage.Margin = new Padding(4, 3, 4, 3);
            DebugNugetPackage.Name = "DebugNugetPackage";
            DebugNugetPackage.Size = new Size(150, 19);
            DebugNugetPackage.TabIndex = 16;
            DebugNugetPackage.Text = "Debug NuGet Package?";
            DebugNugetPackage.UseVisualStyleBackColor = true;
            // 
            // Prerelease
            // 
            Prerelease.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Prerelease.Location = new Point(181, 804);
            Prerelease.Margin = new Padding(4, 3, 4, 3);
            Prerelease.Name = "Prerelease";
            Prerelease.Size = new Size(151, 23);
            Prerelease.TabIndex = 13;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(177, 786);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 12;
            label2.Text = "Pre-release";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.Location = new Point(338, 786);
            button1.Margin = new Padding(2, 3, 2, 3);
            button1.Name = "button1";
            button1.Size = new Size(107, 42);
            button1.TabIndex = 14;
            button1.Text = "Set pre-release";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PackagerView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1176, 841);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(Prerelease);
            Controls.Add(DebugNugetPackage);
            Controls.Add(NuGetVersionsButton);
            Controls.Add(ClearUsagesButton);
            Controls.Add(ClearButton);
            Controls.Add(InvertButton);
            Controls.Add(ReleaseButton);
            Controls.Add(PackageButton);
            Controls.Add(PackageTabs);
            Controls.Add(ResetButton);
            Controls.Add(PatchButton);
            Controls.Add(MinorButton);
            Controls.Add(MajorButton);
            Controls.Add(BuildButton);
            Controls.Add(FetchButton);
            Controls.Add(FolderButton);
            Controls.Add(Folder);
            Controls.Add(label1);
            Margin = new Padding(2, 3, 2, 3);
            MinimumSize = new Size(1192, 880);
            Name = "PackagerView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Shuttle Packager";
            FormClosing += PackagerView_FormClosing;
            PackageTabs.ResumeLayout(false);
            PackageTab.ResumeLayout(false);
            BuildLogTab.ResumeLayout(false);
            BuildLogTab.PerformLayout();
            PackageContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Folder;
        private System.Windows.Forms.Button FolderButton;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.Button FetchButton;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.Button MajorButton;
        private System.Windows.Forms.Button MinorButton;
        private System.Windows.Forms.Button PatchButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.TabControl PackageTabs;
        private System.Windows.Forms.TabPage PackageTab;
        private System.Windows.Forms.ListView Packages;
        private System.Windows.Forms.ColumnHeader PackageNameColumn;
        private System.Windows.Forms.ColumnHeader VersionColumn;
        private System.Windows.Forms.TabPage BuildLogTab;
        private System.Windows.Forms.TextBox BuildLog;
        private System.Windows.Forms.Button PackageButton;
        private System.Windows.Forms.Button ReleaseButton;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.ContextMenuStrip PackageContextMenu;
        private System.Windows.Forms.ToolStripMenuItem UpdateUsagesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowLogMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MarkUsagesMenuItem;
        private System.Windows.Forms.Button InvertButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.ColumnHeader UsageColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ColumnHeader LocationColumn;
        private System.Windows.Forms.Button ClearUsagesButton;
        private System.Windows.Forms.ToolStripMenuItem ShowUsagesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemoveFromNugetCacheMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem GitHubMenuItem;
        private System.Windows.Forms.ColumnHeader NuGetVersionColumn;
        private System.Windows.Forms.Button NuGetVersionsButton;
        private System.Windows.Forms.CheckBox DebugNugetPackage;
        private System.Windows.Forms.TextBox Prerelease;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

