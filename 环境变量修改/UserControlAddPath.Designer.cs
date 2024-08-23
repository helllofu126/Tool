namespace 环境变量修改
{
    partial class UserControlAddPath
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
            this.groupBoxSetting = new System.Windows.Forms.GroupBox();
            this.listViewShow = new System.Windows.Forms.ListView();
            this.radioButtonPath = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.labelHint = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonBrowseFile = new System.Windows.Forms.Button();
            this.buttonBrowseDirectory = new System.Windows.Forms.Button();
            this.textBoxVariableValue = new System.Windows.Forms.TextBox();
            this.comboBoxVariableName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSetting
            // 
            this.groupBoxSetting.Controls.Add(this.listViewShow);
            this.groupBoxSetting.Controls.Add(this.radioButtonPath);
            this.groupBoxSetting.Controls.Add(this.radioButtonAll);
            this.groupBoxSetting.Controls.Add(this.labelHint);
            this.groupBoxSetting.Controls.Add(this.buttonCancel);
            this.groupBoxSetting.Controls.Add(this.buttonAdd);
            this.groupBoxSetting.Controls.Add(this.buttonBrowseFile);
            this.groupBoxSetting.Controls.Add(this.buttonBrowseDirectory);
            this.groupBoxSetting.Controls.Add(this.textBoxVariableValue);
            this.groupBoxSetting.Controls.Add(this.comboBoxVariableName);
            this.groupBoxSetting.Controls.Add(this.label2);
            this.groupBoxSetting.Controls.Add(this.label1);
            this.groupBoxSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSetting.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSetting.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxSetting.Name = "groupBoxSetting";
            this.groupBoxSetting.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxSetting.Size = new System.Drawing.Size(716, 334);
            this.groupBoxSetting.TabIndex = 0;
            this.groupBoxSetting.TabStop = false;
            this.groupBoxSetting.Text = "groupBox1";
            // 
            // listViewShow
            // 
            this.listViewShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewShow.BackColor = System.Drawing.SystemColors.Window;
            this.listViewShow.FullRowSelect = true;
            this.listViewShow.GridLines = true;
            this.listViewShow.HideSelection = false;
            this.listViewShow.Location = new System.Drawing.Point(3, 149);
            this.listViewShow.Name = "listViewShow";
            this.listViewShow.Size = new System.Drawing.Size(697, 178);
            this.listViewShow.TabIndex = 11;
            this.listViewShow.UseCompatibleStateImageBehavior = false;
            this.listViewShow.View = System.Windows.Forms.View.Details;
            this.listViewShow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewShow_KeyDown);
            // 
            // radioButtonPath
            // 
            this.radioButtonPath.AutoSize = true;
            this.radioButtonPath.Location = new System.Drawing.Point(49, 124);
            this.radioButtonPath.Name = "radioButtonPath";
            this.radioButtonPath.Size = new System.Drawing.Size(49, 19);
            this.radioButtonPath.TabIndex = 10;
            this.radioButtonPath.TabStop = true;
            this.radioButtonPath.Text = "Path";
            this.radioButtonPath.UseVisualStyleBackColor = true;
            this.radioButtonPath.CheckedChanged += new System.EventHandler(this.radioButtonPath_CheckedChanged);
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(4, 124);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(39, 19);
            this.radioButtonAll.TabIndex = 9;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.CheckedChanged += new System.EventHandler(this.radioButtonAll_CheckedChanged);
            // 
            // labelHint
            // 
            this.labelHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHint.AutoSize = true;
            this.labelHint.Location = new System.Drawing.Point(259, 96);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(46, 15);
            this.labelHint.TabIndex = 8;
            this.labelHint.Text = "提示：";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(605, 92);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(95, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(506, 92);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(97, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "创建";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonBrowseFile
            // 
            this.buttonBrowseFile.Location = new System.Drawing.Point(120, 90);
            this.buttonBrowseFile.Name = "buttonBrowseFile";
            this.buttonBrowseFile.Size = new System.Drawing.Size(111, 25);
            this.buttonBrowseFile.TabIndex = 5;
            this.buttonBrowseFile.Text = "浏览文件(F)...";
            this.buttonBrowseFile.UseVisualStyleBackColor = true;
            this.buttonBrowseFile.Click += new System.EventHandler(this.buttonBrowseFile_Click);
            // 
            // buttonBrowseDirectory
            // 
            this.buttonBrowseDirectory.Location = new System.Drawing.Point(7, 90);
            this.buttonBrowseDirectory.Name = "buttonBrowseDirectory";
            this.buttonBrowseDirectory.Size = new System.Drawing.Size(111, 25);
            this.buttonBrowseDirectory.TabIndex = 4;
            this.buttonBrowseDirectory.Text = "浏览目录(D)...";
            this.buttonBrowseDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseDirectory.Click += new System.EventHandler(this.buttonBrowseDirectory_Click);
            // 
            // textBoxVariableValue
            // 
            this.textBoxVariableValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVariableValue.Location = new System.Drawing.Point(72, 55);
            this.textBoxVariableValue.Name = "textBoxVariableValue";
            this.textBoxVariableValue.Size = new System.Drawing.Size(620, 23);
            this.textBoxVariableValue.TabIndex = 3;
            // 
            // comboBoxVariableName
            // 
            this.comboBoxVariableName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVariableName.FormattingEnabled = true;
            this.comboBoxVariableName.Location = new System.Drawing.Point(72, 17);
            this.comboBoxVariableName.Name = "comboBoxVariableName";
            this.comboBoxVariableName.Size = new System.Drawing.Size(620, 23);
            this.comboBoxVariableName.TabIndex = 2;
            this.comboBoxVariableName.TextChanged += new System.EventHandler(this.comboBoxVariableName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "变量值(V):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "变量名(N):";
            // 
            // UserControlAddPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSetting);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserControlAddPath";
            this.Size = new System.Drawing.Size(716, 334);
            this.groupBoxSetting.ResumeLayout(false);
            this.groupBoxSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSetting;
        private System.Windows.Forms.Button buttonBrowseFile;
        private System.Windows.Forms.Button buttonBrowseDirectory;
        private System.Windows.Forms.TextBox textBoxVariableValue;
        private System.Windows.Forms.ComboBox comboBoxVariableName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListView listViewShow;
        private System.Windows.Forms.RadioButton radioButtonPath;
        private System.Windows.Forms.RadioButton radioButtonAll;
    }
}
