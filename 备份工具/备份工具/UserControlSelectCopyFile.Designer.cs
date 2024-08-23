namespace HelloFu.BackupTool
{
    partial class UserControlSelectCopyFile
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSelectCopyFile));
            this.crossLineLabelSelectFile = new HelloFu.BackupTool.CrossLineLabel();
            this.listBoxFilePath = new System.Windows.Forms.ListBox();
            this.contextMenuStripDelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.delectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerRefreshPath = new System.Windows.Forms.Timer(this.components);
            this.labelListBoxName = new System.Windows.Forms.Label();
            this.contextMenuStripDelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // crossLineLabelSelectFile
            // 
            this.crossLineLabelSelectFile.AllowDrop = true;
            this.crossLineLabelSelectFile.FilePathList = ((System.Collections.Generic.List<string>)(resources.GetObject("crossLineLabelSelectFile.FilePathList")));
            this.crossLineLabelSelectFile.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crossLineLabelSelectFile.Location = new System.Drawing.Point(14, 11);
            this.crossLineLabelSelectFile.Name = "crossLineLabelSelectFile";
            this.crossLineLabelSelectFile.Size = new System.Drawing.Size(555, 320);
            this.crossLineLabelSelectFile.TabIndex = 0;
            // 
            // listBoxFilePath
            // 
            this.listBoxFilePath.ContextMenuStrip = this.contextMenuStripDelect;
            this.listBoxFilePath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxFilePath.FormattingEnabled = true;
            this.listBoxFilePath.HorizontalScrollbar = true;
            this.listBoxFilePath.ItemHeight = 25;
            this.listBoxFilePath.Location = new System.Drawing.Point(575, 50);
            this.listBoxFilePath.Name = "listBoxFilePath";
            this.listBoxFilePath.ScrollAlwaysVisible = true;
            this.listBoxFilePath.Size = new System.Drawing.Size(625, 279);
            this.listBoxFilePath.TabIndex = 1;
            this.listBoxFilePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxFilePath_KeyDown);
            // 
            // contextMenuStripDelect
            // 
            this.contextMenuStripDelect.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripDelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delectToolStripMenuItem});
            this.contextMenuStripDelect.Name = "contextMenuStripDelect";
            this.contextMenuStripDelect.Size = new System.Drawing.Size(136, 34);
            // 
            // delectToolStripMenuItem
            // 
            this.delectToolStripMenuItem.Name = "delectToolStripMenuItem";
            this.delectToolStripMenuItem.Size = new System.Drawing.Size(135, 30);
            this.delectToolStripMenuItem.Text = "Delect";
            this.delectToolStripMenuItem.Click += new System.EventHandler(this.delectToolStripMenuItem_Click);
            // 
            // labelListBoxName
            // 
            this.labelListBoxName.AutoSize = true;
            this.labelListBoxName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelListBoxName.Location = new System.Drawing.Point(780, 7);
            this.labelListBoxName.Name = "labelListBoxName";
            this.labelListBoxName.Size = new System.Drawing.Size(249, 38);
            this.labelListBoxName.TabIndex = 2;
            this.labelListBoxName.Text = "待压缩的文件路径";
            // 
            // UserControlSelectCopyFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelListBoxName);
            this.Controls.Add(this.listBoxFilePath);
            this.Controls.Add(this.crossLineLabelSelectFile);
            this.Name = "UserControlSelectCopyFile";
            this.Size = new System.Drawing.Size(1211, 354);
            this.Load += new System.EventHandler(this.UserControlSelectCopyFile_Load);
            this.contextMenuStripDelect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrossLineLabel crossLineLabelSelectFile;
        private System.Windows.Forms.Timer timerRefreshPath;
        private System.Windows.Forms.ListBox listBoxFilePath;
        private System.Windows.Forms.Label labelListBoxName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDelect;
        private System.Windows.Forms.ToolStripMenuItem delectToolStripMenuItem;
    }
}
