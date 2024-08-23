namespace Wes.Simulator
{
    sealed partial class UserControlTabPage
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
            this.textBoxJson = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelMembers = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxRequestType = new System.Windows.Forms.TextBox();
            this.textBoxRequestTypeInt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxJson
            // 
            this.textBoxJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxJson.Location = new System.Drawing.Point(0, 0);
            this.textBoxJson.Multiline = true;
            this.textBoxJson.Name = "textBoxJson";
            this.textBoxJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxJson.Size = new System.Drawing.Size(420, 394);
            this.textBoxJson.TabIndex = 1;
            // 
            // flowLayoutPanelMembers
            // 
            this.flowLayoutPanelMembers.AutoScroll = true;
            this.flowLayoutPanelMembers.AutoSize = true;
            this.flowLayoutPanelMembers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelMembers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelMembers.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelMembers.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelMembers.Name = "flowLayoutPanelMembers";
            this.flowLayoutPanelMembers.Size = new System.Drawing.Size(338, 394);
            this.flowLayoutPanelMembers.TabIndex = 2;
            this.flowLayoutPanelMembers.WrapContents = false;
            this.flowLayoutPanelMembers.SizeChanged += new System.EventHandler(this.flowLayoutPanelMembers_SizeChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 36);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanelMembers);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxJson);
            this.splitContainer1.Size = new System.Drawing.Size(766, 396);
            this.splitContainer1.SplitterDistance = 340;
            this.splitContainer1.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSend.BackColor = System.Drawing.Color.YellowGreen;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.Location = new System.Drawing.Point(0, 434);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(100, 29);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonSend_MouseClick);
            // 
            // textBoxRequestType
            // 
            this.textBoxRequestType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRequestType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRequestType.Location = new System.Drawing.Point(3, 7);
            this.textBoxRequestType.Name = "textBoxRequestType";
            this.textBoxRequestType.Size = new System.Drawing.Size(580, 23);
            this.textBoxRequestType.TabIndex = 5;
            // 
            // textBoxRequestTypeInt
            // 
            this.textBoxRequestTypeInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRequestTypeInt.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRequestTypeInt.Location = new System.Drawing.Point(608, 5);
            this.textBoxRequestTypeInt.Name = "textBoxRequestTypeInt";
            this.textBoxRequestTypeInt.Size = new System.Drawing.Size(155, 26);
            this.textBoxRequestTypeInt.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(583, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 21);
            this.label4.TabIndex = 9;
            this.label4.Text = "=";
            // 
            // UserControlTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.textBoxRequestTypeInt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxRequestType);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserControlTabPage";
            this.Size = new System.Drawing.Size(766, 464);
            this.Load += new System.EventHandler(this.UserControlTabPage_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxJson;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMembers;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxRequestType;
        private System.Windows.Forms.TextBox textBoxRequestTypeInt;
        private System.Windows.Forms.Label label4;
    }
}
