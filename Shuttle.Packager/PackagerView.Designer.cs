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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackagerView));
            this.label1 = new System.Windows.Forms.Label();
            this.Folder = new System.Windows.Forms.TextBox();
            this.FolderButton = new System.Windows.Forms.Button();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.FetchButton = new System.Windows.Forms.Button();
            this.BuildButton = new System.Windows.Forms.Button();
            this.MajorButton = new System.Windows.Forms.Button();
            this.MinorButton = new System.Windows.Forms.Button();
            this.PatchButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.PackageTabs = new System.Windows.Forms.TabControl();
            this.PackageTab = new System.Windows.Forms.TabPage();
            this.Packages = new System.Windows.Forms.ListView();
            this.PackageNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NuGetVersionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UsageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LocationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.BuildLogTab = new System.Windows.Forms.TabPage();
            this.BuildLog = new System.Windows.Forms.TextBox();
            this.PackageButton = new System.Windows.Forms.Button();
            this.ReleaseButton = new System.Windows.Forms.Button();
            this.PackageContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UpdateUsagesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MarkUsagesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowUsagesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GitHubMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveFromNugetCacheMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InvertButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.ClearUsagesButton = new System.Windows.Forms.Button();
            this.NuGetVersionsButton = new System.Windows.Forms.Button();
            this.DebugNugetPackage = new System.Windows.Forms.CheckBox();
            this.PackageTabs.SuspendLayout();
            this.PackageTab.SuspendLayout();
            this.BuildLogTab.SuspendLayout();
            this.PackageContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Folder";
            // 
            // Folder
            // 
            this.Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Folder.Location = new System.Drawing.Point(15, 32);
            this.Folder.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(720, 20);
            this.Folder.TabIndex = 2;
            this.Folder.Text = "D:\\development.github\\shuttle";
            // 
            // FolderButton
            // 
            this.FolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderButton.Location = new System.Drawing.Point(660, 65);
            this.FolderButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.FolderButton.Name = "FolderButton";
            this.FolderButton.Size = new System.Drawing.Size(75, 32);
            this.FolderButton.TabIndex = 3;
            this.FolderButton.Text = "Select";
            this.FolderButton.UseVisualStyleBackColor = true;
            this.FolderButton.Click += new System.EventHandler(this.FolderButton_Click);
            // 
            // FetchButton
            // 
            this.FetchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FetchButton.Location = new System.Drawing.Point(570, 65);
            this.FetchButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.FetchButton.Name = "FetchButton";
            this.FetchButton.Size = new System.Drawing.Size(75, 32);
            this.FetchButton.TabIndex = 4;
            this.FetchButton.Text = "Fetch";
            this.FetchButton.UseVisualStyleBackColor = true;
            this.FetchButton.Click += new System.EventHandler(this.FetchButton_Click);
            // 
            // BuildButton
            // 
            this.BuildButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuildButton.Location = new System.Drawing.Point(658, 497);
            this.BuildButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(75, 32);
            this.BuildButton.TabIndex = 6;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // MajorButton
            // 
            this.MajorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MajorButton.Location = new System.Drawing.Point(15, 497);
            this.MajorButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MajorButton.Name = "MajorButton";
            this.MajorButton.Size = new System.Drawing.Size(45, 32);
            this.MajorButton.TabIndex = 7;
            this.MajorButton.Text = "Major";
            this.MajorButton.UseVisualStyleBackColor = true;
            this.MajorButton.Click += new System.EventHandler(this.MajorButton_Click);
            // 
            // MinorButton
            // 
            this.MinorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MinorButton.Location = new System.Drawing.Point(60, 497);
            this.MinorButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MinorButton.Name = "MinorButton";
            this.MinorButton.Size = new System.Drawing.Size(45, 32);
            this.MinorButton.TabIndex = 8;
            this.MinorButton.Text = "Minor";
            this.MinorButton.UseVisualStyleBackColor = true;
            this.MinorButton.Click += new System.EventHandler(this.MinorButton_Click);
            // 
            // PatchButton
            // 
            this.PatchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PatchButton.Location = new System.Drawing.Point(105, 497);
            this.PatchButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PatchButton.Name = "PatchButton";
            this.PatchButton.Size = new System.Drawing.Size(45, 32);
            this.PatchButton.TabIndex = 9;
            this.PatchButton.Text = "Patch";
            this.PatchButton.UseVisualStyleBackColor = true;
            this.PatchButton.Click += new System.EventHandler(this.PatchButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResetButton.Location = new System.Drawing.Point(165, 497);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(45, 32);
            this.ResetButton.TabIndex = 10;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // PackageTabs
            // 
            this.PackageTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PackageTabs.Controls.Add(this.PackageTab);
            this.PackageTabs.Controls.Add(this.BuildLogTab);
            this.PackageTabs.Location = new System.Drawing.Point(15, 114);
            this.PackageTabs.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PackageTabs.Name = "PackageTabs";
            this.PackageTabs.SelectedIndex = 0;
            this.PackageTabs.Size = new System.Drawing.Size(718, 367);
            this.PackageTabs.TabIndex = 11;
            // 
            // PackageTab
            // 
            this.PackageTab.Controls.Add(this.Packages);
            this.PackageTab.Location = new System.Drawing.Point(4, 22);
            this.PackageTab.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PackageTab.Name = "PackageTab";
            this.PackageTab.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PackageTab.Size = new System.Drawing.Size(710, 341);
            this.PackageTab.TabIndex = 0;
            this.PackageTab.Text = "Packages";
            this.PackageTab.UseVisualStyleBackColor = true;
            // 
            // Packages
            // 
            this.Packages.CheckBoxes = true;
            this.Packages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PackageNameColumn,
            this.VersionColumn,
            this.NuGetVersionColumn,
            this.UsageColumn,
            this.LocationColumn});
            this.Packages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Packages.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Packages.FullRowSelect = true;
            this.Packages.HideSelection = false;
            this.Packages.Location = new System.Drawing.Point(2, 3);
            this.Packages.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Packages.Name = "Packages";
            this.Packages.Size = new System.Drawing.Size(706, 335);
            this.Packages.SmallImageList = this.ImageList;
            this.Packages.TabIndex = 1;
            this.Packages.UseCompatibleStateImageBehavior = false;
            this.Packages.View = System.Windows.Forms.View.Details;
            this.Packages.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Packages_MouseClick);
            this.Packages.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Packages_MouseDoubleClick);
            // 
            // PackageNameColumn
            // 
            this.PackageNameColumn.Text = "Package";
            this.PackageNameColumn.Width = 200;
            // 
            // VersionColumn
            // 
            this.VersionColumn.Text = "Version";
            this.VersionColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.VersionColumn.Width = 100;
            // 
            // NuGetVersionColumn
            // 
            this.NuGetVersionColumn.Text = "NuGet Version";
            this.NuGetVersionColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NuGetVersionColumn.Width = 120;
            // 
            // UsageColumn
            // 
            this.UsageColumn.Text = "Usage";
            this.UsageColumn.Width = 75;
            // 
            // LocationColumn
            // 
            this.LocationColumn.Text = "Location";
            this.LocationColumn.Width = 200;
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "hourglass");
            this.ImageList.Images.SetKeyName(1, "tick");
            this.ImageList.Images.SetKeyName(2, "cross");
            this.ImageList.Images.SetKeyName(3, "package");
            // 
            // BuildLogTab
            // 
            this.BuildLogTab.Controls.Add(this.BuildLog);
            this.BuildLogTab.Location = new System.Drawing.Point(4, 22);
            this.BuildLogTab.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BuildLogTab.Name = "BuildLogTab";
            this.BuildLogTab.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BuildLogTab.Size = new System.Drawing.Size(637, 348);
            this.BuildLogTab.TabIndex = 1;
            this.BuildLogTab.Text = "Log";
            this.BuildLogTab.UseVisualStyleBackColor = true;
            // 
            // BuildLog
            // 
            this.BuildLog.BackColor = System.Drawing.Color.Black;
            this.BuildLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BuildLog.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BuildLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BuildLog.Location = new System.Drawing.Point(2, 3);
            this.BuildLog.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BuildLog.Multiline = true;
            this.BuildLog.Name = "BuildLog";
            this.BuildLog.ReadOnly = true;
            this.BuildLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.BuildLog.Size = new System.Drawing.Size(633, 342);
            this.BuildLog.TabIndex = 0;
            // 
            // PackageButton
            // 
            this.PackageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PackageButton.Location = new System.Drawing.Point(568, 497);
            this.PackageButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PackageButton.Name = "PackageButton";
            this.PackageButton.Size = new System.Drawing.Size(75, 32);
            this.PackageButton.TabIndex = 12;
            this.PackageButton.Text = "Package";
            this.PackageButton.UseVisualStyleBackColor = true;
            this.PackageButton.Click += new System.EventHandler(this.PackageButton_Click);
            // 
            // ReleaseButton
            // 
            this.ReleaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReleaseButton.Location = new System.Drawing.Point(478, 497);
            this.ReleaseButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ReleaseButton.Name = "ReleaseButton";
            this.ReleaseButton.Size = new System.Drawing.Size(75, 32);
            this.ReleaseButton.TabIndex = 13;
            this.ReleaseButton.Text = "Release";
            this.ReleaseButton.UseVisualStyleBackColor = true;
            this.ReleaseButton.Click += new System.EventHandler(this.ReleaseButton_Click);
            // 
            // PackageContextMenu
            // 
            this.PackageContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.PackageContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateUsagesMenuItem,
            this.MarkUsagesMenuItem,
            this.ShowUsagesMenuItem,
            this.toolStripSeparator2,
            this.OpenMenuItem,
            this.GitHubMenuItem,
            this.toolStripSeparator1,
            this.RemoveFromNugetCacheMenuItem,
            this.toolStripSeparator3,
            this.ShowLogMenuItem});
            this.PackageContextMenu.Name = "ItemContextMenu";
            this.PackageContextMenu.Size = new System.Drawing.Size(217, 176);
            // 
            // UpdateUsagesMenuItem
            // 
            this.UpdateUsagesMenuItem.Name = "UpdateUsagesMenuItem";
            this.UpdateUsagesMenuItem.Size = new System.Drawing.Size(216, 22);
            this.UpdateUsagesMenuItem.Text = "&Update usages";
            // 
            // MarkUsagesMenuItem
            // 
            this.MarkUsagesMenuItem.Name = "MarkUsagesMenuItem";
            this.MarkUsagesMenuItem.Size = new System.Drawing.Size(216, 22);
            this.MarkUsagesMenuItem.Text = "&Mark usages";
            // 
            // ShowUsagesMenuItem
            // 
            this.ShowUsagesMenuItem.Name = "ShowUsagesMenuItem";
            this.ShowUsagesMenuItem.Size = new System.Drawing.Size(216, 22);
            this.ShowUsagesMenuItem.Text = "&Show usages";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Size = new System.Drawing.Size(216, 22);
            this.OpenMenuItem.Text = "&Open";
            // 
            // GitHubMenuItem
            // 
            this.GitHubMenuItem.Name = "GitHubMenuItem";
            this.GitHubMenuItem.Size = new System.Drawing.Size(216, 22);
            this.GitHubMenuItem.Text = "GitHub";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // RemoveFromNugetCacheMenuItem
            // 
            this.RemoveFromNugetCacheMenuItem.Name = "RemoveFromNugetCacheMenuItem";
            this.RemoveFromNugetCacheMenuItem.Size = new System.Drawing.Size(216, 22);
            this.RemoveFromNugetCacheMenuItem.Text = "&Remove from Nuget cache";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
            // 
            // ShowLogMenuItem
            // 
            this.ShowLogMenuItem.Name = "ShowLogMenuItem";
            this.ShowLogMenuItem.Size = new System.Drawing.Size(216, 22);
            this.ShowLogMenuItem.Text = "Show &log";
            // 
            // InvertButton
            // 
            this.InvertButton.Location = new System.Drawing.Point(15, 65);
            this.InvertButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.InvertButton.Name = "InvertButton";
            this.InvertButton.Size = new System.Drawing.Size(75, 32);
            this.InvertButton.TabIndex = 14;
            this.InvertButton.Text = "Invert";
            this.InvertButton.UseVisualStyleBackColor = true;
            this.InvertButton.Click += new System.EventHandler(this.InvertButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(105, 65);
            this.ClearButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 32);
            this.ClearButton.TabIndex = 15;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // ClearUsagesButton
            // 
            this.ClearUsagesButton.Location = new System.Drawing.Point(195, 65);
            this.ClearUsagesButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ClearUsagesButton.Name = "ClearUsagesButton";
            this.ClearUsagesButton.Size = new System.Drawing.Size(90, 32);
            this.ClearUsagesButton.TabIndex = 16;
            this.ClearUsagesButton.Text = "Clear usages";
            this.ClearUsagesButton.UseVisualStyleBackColor = true;
            this.ClearUsagesButton.Click += new System.EventHandler(this.ClearUsagesButton_Click);
            // 
            // NuGetVersionsButton
            // 
            this.NuGetVersionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NuGetVersionsButton.Location = new System.Drawing.Point(452, 65);
            this.NuGetVersionsButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.NuGetVersionsButton.Name = "NuGetVersionsButton";
            this.NuGetVersionsButton.Size = new System.Drawing.Size(101, 32);
            this.NuGetVersionsButton.TabIndex = 17;
            this.NuGetVersionsButton.Text = "NuGet Versions";
            this.NuGetVersionsButton.UseVisualStyleBackColor = true;
            this.NuGetVersionsButton.Click += new System.EventHandler(this.NuGetVersionsButton_Click);
            // 
            // DebugNugetPackage
            // 
            this.DebugNugetPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugNugetPackage.AutoSize = true;
            this.DebugNugetPackage.Checked = true;
            this.DebugNugetPackage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DebugNugetPackage.Location = new System.Drawing.Point(329, 506);
            this.DebugNugetPackage.Name = "DebugNugetPackage";
            this.DebugNugetPackage.Size = new System.Drawing.Size(144, 17);
            this.DebugNugetPackage.TabIndex = 18;
            this.DebugNugetPackage.Text = "Debug NuGet Package?";
            this.DebugNugetPackage.UseVisualStyleBackColor = true;
            // 
            // PackagerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 545);
            this.Controls.Add(this.DebugNugetPackage);
            this.Controls.Add(this.NuGetVersionsButton);
            this.Controls.Add(this.ClearUsagesButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.InvertButton);
            this.Controls.Add(this.ReleaseButton);
            this.Controls.Add(this.PackageButton);
            this.Controls.Add(this.PackageTabs);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.PatchButton);
            this.Controls.Add(this.MinorButton);
            this.Controls.Add(this.MajorButton);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.FetchButton);
            this.Controls.Add(this.FolderButton);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "PackagerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shuttle Packager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PackagerView_FormClosing);
            this.PackageTabs.ResumeLayout(false);
            this.PackageTab.ResumeLayout(false);
            this.BuildLogTab.ResumeLayout(false);
            this.BuildLogTab.PerformLayout();
            this.PackageContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

