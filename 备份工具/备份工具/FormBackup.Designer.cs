namespace HelloFu.BackupTool
{
    partial class FormBackup
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBackup = new System.Windows.Forms.Button();
            this.userControlSelectCopyFileBackup = new HelloFu.BackupTool.UserControlSelectCopyFile();
            this.userControlSelectCopyFilePath = new HelloFu.BackupTool.UserControlSelectCopyFile();
            this.textBoxBackupFile = new System.Windows.Forms.TextBox();
            this.labelBackupName = new System.Windows.Forms.Label();
            this.labelBackup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBackup
            // 
            this.buttonBackup.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBackup.Location = new System.Drawing.Point(798, 696);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(159, 66);
            this.buttonBackup.TabIndex = 2;
            this.buttonBackup.Text = "备份文件";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // userControlSelectCopyFileBackup
            // 
            this.userControlSelectCopyFileBackup.Location = new System.Drawing.Point(12, 357);
            this.userControlSelectCopyFileBackup.Name = "userControlSelectCopyFileBackup";
            this.userControlSelectCopyFileBackup.Size = new System.Drawing.Size(1213, 353);
            this.userControlSelectCopyFileBackup.TabIndex = 1;
            // 
            // userControlSelectCopyFilePath
            // 
            this.userControlSelectCopyFilePath.Location = new System.Drawing.Point(12, 12);
            this.userControlSelectCopyFilePath.Name = "userControlSelectCopyFilePath";
            this.userControlSelectCopyFilePath.Size = new System.Drawing.Size(1213, 353);
            this.userControlSelectCopyFilePath.TabIndex = 0;
            // 
            // textBoxBackupFile
            // 
            this.textBoxBackupFile.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBackupFile.Location = new System.Drawing.Point(236, 710);
            this.textBoxBackupFile.Multiline = true;
            this.textBoxBackupFile.Name = "textBoxBackupFile";
            this.textBoxBackupFile.Size = new System.Drawing.Size(541, 41);
            this.textBoxBackupFile.TabIndex = 3;
            this.textBoxBackupFile.Text = "新建备份";
            // 
            // labelBackupName
            // 
            this.labelBackupName.AutoSize = true;
            this.labelBackupName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBackupName.Location = new System.Drawing.Point(111, 713);
            this.labelBackupName.Name = "labelBackupName";
            this.labelBackupName.Size = new System.Drawing.Size(119, 32);
            this.labelBackupName.TabIndex = 4;
            this.labelBackupName.Text = "备份名称:";
            // 
            // labelBackup
            // 
            this.labelBackup.AutoSize = true;
            this.labelBackup.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBackup.Location = new System.Drawing.Point(963, 713);
            this.labelBackup.Name = "labelBackup";
            this.labelBackup.Size = new System.Drawing.Size(0, 48);
            this.labelBackup.TabIndex = 5;
            // 
            // FormBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 806);
            this.Controls.Add(this.labelBackup);
            this.Controls.Add(this.labelBackupName);
            this.Controls.Add(this.textBoxBackupFile);
            this.Controls.Add(this.buttonBackup);
            this.Controls.Add(this.userControlSelectCopyFileBackup);
            this.Controls.Add(this.userControlSelectCopyFilePath);
            this.Name = "FormBackup";
            this.Text = "备份工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControlSelectCopyFile userControlSelectCopyFilePath;
        private UserControlSelectCopyFile userControlSelectCopyFileBackup;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.TextBox textBoxBackupFile;
        private System.Windows.Forms.Label labelBackupName;
        private System.Windows.Forms.Label labelBackup;
    }
}

