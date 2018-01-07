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
            this.Packages = new System.Windows.Forms.ListView();
            this.PackageNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LocationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.SuspendLayout();
            // 
            // Packages
            // 
            this.Packages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Packages.CheckBoxes = true;
            this.Packages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PackageNameColumn,
            this.VersionColumn,
            this.LocationColumn});
            this.Packages.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Packages.FullRowSelect = true;
            this.Packages.HideSelection = false;
            this.Packages.Location = new System.Drawing.Point(20, 140);
            this.Packages.Name = "Packages";
            this.Packages.Size = new System.Drawing.Size(862, 460);
            this.Packages.TabIndex = 0;
            this.Packages.UseCompatibleStateImageBehavior = false;
            this.Packages.View = System.Windows.Forms.View.Details;
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
            this.VersionColumn.Width = 120;
            // 
            // LocationColumn
            // 
            this.LocationColumn.Width = 200;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Folder";
            // 
            // Folder
            // 
            this.Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Folder.Location = new System.Drawing.Point(20, 40);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(862, 22);
            this.Folder.TabIndex = 2;
            this.Folder.Text = "C:\\development.github\\shuttle";
            // 
            // FolderButton
            // 
            this.FolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderButton.Location = new System.Drawing.Point(782, 80);
            this.FolderButton.Name = "FolderButton";
            this.FolderButton.Size = new System.Drawing.Size(100, 40);
            this.FolderButton.TabIndex = 3;
            this.FolderButton.Text = "Select";
            this.FolderButton.UseVisualStyleBackColor = true;
            this.FolderButton.Click += new System.EventHandler(this.FolderButton_Click);
            // 
            // FetchButton
            // 
            this.FetchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FetchButton.Location = new System.Drawing.Point(662, 80);
            this.FetchButton.Name = "FetchButton";
            this.FetchButton.Size = new System.Drawing.Size(100, 40);
            this.FetchButton.TabIndex = 4;
            this.FetchButton.Text = "Fetch";
            this.FetchButton.UseVisualStyleBackColor = true;
            this.FetchButton.Click += new System.EventHandler(this.FetchButton_Click);
            // 
            // BuildButton
            // 
            this.BuildButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuildButton.Location = new System.Drawing.Point(780, 620);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(100, 40);
            this.BuildButton.TabIndex = 6;
            this.BuildButton.Text = "Build";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // MajorButton
            // 
            this.MajorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MajorButton.Location = new System.Drawing.Point(20, 620);
            this.MajorButton.Name = "MajorButton";
            this.MajorButton.Size = new System.Drawing.Size(60, 40);
            this.MajorButton.TabIndex = 7;
            this.MajorButton.Text = "Major";
            this.MajorButton.UseVisualStyleBackColor = true;
            this.MajorButton.Click += new System.EventHandler(this.MajorButton_Click);
            // 
            // MinorButton
            // 
            this.MinorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MinorButton.Location = new System.Drawing.Point(80, 620);
            this.MinorButton.Name = "MinorButton";
            this.MinorButton.Size = new System.Drawing.Size(60, 40);
            this.MinorButton.TabIndex = 8;
            this.MinorButton.Text = "Minor";
            this.MinorButton.UseVisualStyleBackColor = true;
            this.MinorButton.Click += new System.EventHandler(this.MinorButton_Click);
            // 
            // PatchButton
            // 
            this.PatchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PatchButton.Location = new System.Drawing.Point(140, 620);
            this.PatchButton.Name = "PatchButton";
            this.PatchButton.Size = new System.Drawing.Size(60, 40);
            this.PatchButton.TabIndex = 9;
            this.PatchButton.Text = "Patch";
            this.PatchButton.UseVisualStyleBackColor = true;
            this.PatchButton.Click += new System.EventHandler(this.PatchButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResetButton.Location = new System.Drawing.Point(220, 620);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(60, 40);
            this.ResetButton.TabIndex = 10;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // PackagerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 679);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.PatchButton);
            this.Controls.Add(this.MinorButton);
            this.Controls.Add(this.MajorButton);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.FetchButton);
            this.Controls.Add(this.FolderButton);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Packages);
            this.Name = "PackagerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shuttle Packager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView Packages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Folder;
        private System.Windows.Forms.Button FolderButton;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.Button FetchButton;
        private System.Windows.Forms.ColumnHeader PackageNameColumn;
        private System.Windows.Forms.ColumnHeader VersionColumn;
        private System.Windows.Forms.ColumnHeader LocationColumn;
        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.Button MajorButton;
        private System.Windows.Forms.Button MinorButton;
        private System.Windows.Forms.Button PatchButton;
        private System.Windows.Forms.Button ResetButton;
    }
}

