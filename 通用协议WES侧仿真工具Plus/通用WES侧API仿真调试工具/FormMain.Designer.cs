namespace 通用WES侧API仿真调试工具
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelHostTitle = new System.Windows.Forms.Label();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.buttonAddLocalApi = new System.Windows.Forms.Button();
            this.textBoxReceived = new System.Windows.Forms.TextBox();
            this.textBoxPost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLocalUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxResponse = new System.Windows.Forms.TextBox();
            this.listBoxUrlList = new System.Windows.Forms.ListBox();
            this.textBoxPrintJson = new System.Windows.Forms.TextBox();
            this.userControlTabControl = new WES.Simulator.UserControlTabControl();
            this.SuspendLayout();
            // 
            // labelHostTitle
            // 
            this.labelHostTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHostTitle.AutoSize = true;
            this.labelHostTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHostTitle.Location = new System.Drawing.Point(657, 10);
            this.labelHostTitle.Name = "labelHostTitle";
            this.labelHostTitle.Size = new System.Drawing.Size(63, 15);
            this.labelHostTitle.TabIndex = 6;
            this.labelHostTitle.Text = "Local Port ";
            // 
            // textBoxLocalPort
            // 
            this.textBoxLocalPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLocalPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLocalPort.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLocalPort.Location = new System.Drawing.Point(723, 4);
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.Size = new System.Drawing.Size(299, 26);
            this.textBoxLocalPort.TabIndex = 5;
            this.textBoxLocalPort.Text = "8081";
            this.textBoxLocalPort.TextChanged += new System.EventHandler(this.textBoxLocalPort_TextChanged);
            // 
            // buttonAddLocalApi
            // 
            this.buttonAddLocalApi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddLocalApi.BackColor = System.Drawing.Color.Pink;
            this.buttonAddLocalApi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddLocalApi.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAddLocalApi.Location = new System.Drawing.Point(658, 237);
            this.buttonAddLocalApi.Name = "buttonAddLocalApi";
            this.buttonAddLocalApi.Size = new System.Drawing.Size(364, 31);
            this.buttonAddLocalApi.TabIndex = 7;
            this.buttonAddLocalApi.Text = "添加本地Api";
            this.buttonAddLocalApi.UseVisualStyleBackColor = false;
            this.buttonAddLocalApi.Click += new System.EventHandler(this.buttonAddLocalApi_Click);
            // 
            // textBoxReceived
            // 
            this.textBoxReceived.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceived.BackColor = System.Drawing.Color.White;
            this.textBoxReceived.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxReceived.Location = new System.Drawing.Point(336, 272);
            this.textBoxReceived.Multiline = true;
            this.textBoxReceived.Name = "textBoxReceived";
            this.textBoxReceived.ReadOnly = true;
            this.textBoxReceived.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxReceived.Size = new System.Drawing.Size(316, 192);
            this.textBoxReceived.TabIndex = 10;
            // 
            // textBoxPost
            // 
            this.textBoxPost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPost.BackColor = System.Drawing.Color.White;
            this.textBoxPost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPost.Location = new System.Drawing.Point(3, 272);
            this.textBoxPost.Multiline = true;
            this.textBoxPost.Name = "textBoxPost";
            this.textBoxPost.ReadOnly = true;
            this.textBoxPost.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPost.Size = new System.Drawing.Size(318, 192);
            this.textBoxPost.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(657, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Local URL ";
            // 
            // textBoxLocalUrl
            // 
            this.textBoxLocalUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLocalUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLocalUrl.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLocalUrl.Location = new System.Drawing.Point(723, 36);
            this.textBoxLocalUrl.Name = "textBoxLocalUrl";
            this.textBoxLocalUrl.Size = new System.Drawing.Size(299, 26);
            this.textBoxLocalUrl.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(662, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Response";
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxResponse.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResponse.Location = new System.Drawing.Point(723, 70);
            this.textBoxResponse.Multiline = true;
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxResponse.Size = new System.Drawing.Size(299, 161);
            this.textBoxResponse.TabIndex = 13;
            // 
            // listBoxUrlList
            // 
            this.listBoxUrlList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxUrlList.FormattingEnabled = true;
            this.listBoxUrlList.HorizontalScrollbar = true;
            this.listBoxUrlList.ItemHeight = 15;
            this.listBoxUrlList.Location = new System.Drawing.Point(658, 270);
            this.listBoxUrlList.Name = "listBoxUrlList";
            this.listBoxUrlList.Size = new System.Drawing.Size(125, 199);
            this.listBoxUrlList.TabIndex = 15;
            this.listBoxUrlList.SelectedIndexChanged += new System.EventHandler(this.listBoxUrlList_SelectedIndexChanged);
            this.listBoxUrlList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxUrlList_KeyDown);
            // 
            // textBoxPrintJson
            // 
            this.textBoxPrintJson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrintJson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPrintJson.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPrintJson.Location = new System.Drawing.Point(789, 270);
            this.textBoxPrintJson.Multiline = true;
            this.textBoxPrintJson.Name = "textBoxPrintJson";
            this.textBoxPrintJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPrintJson.Size = new System.Drawing.Size(233, 199);
            this.textBoxPrintJson.TabIndex = 16;
            // 
            // userControlTabControl
            // 
            this.userControlTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlTabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlTabControl.Location = new System.Drawing.Point(4, 6);
            this.userControlTabControl.Margin = new System.Windows.Forms.Padding(4);
            this.userControlTabControl.Name = "userControlTabControl";
            this.userControlTabControl.Size = new System.Drawing.Size(643, 262);
            this.userControlTabControl.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 472);
            this.Controls.Add(this.textBoxPrintJson);
            this.Controls.Add(this.listBoxUrlList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxResponse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLocalUrl);
            this.Controls.Add(this.textBoxReceived);
            this.Controls.Add(this.textBoxPost);
            this.Controls.Add(this.userControlTabControl);
            this.Controls.Add(this.buttonAddLocalApi);
            this.Controls.Add(this.labelHostTitle);
            this.Controls.Add(this.textBoxLocalPort);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHostTitle;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.Button buttonAddLocalApi;
        private WES.Simulator.UserControlTabControl userControlTabControl;
        private System.Windows.Forms.TextBox textBoxReceived;
        private System.Windows.Forms.TextBox textBoxPost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLocalUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxResponse;
        private System.Windows.Forms.ListBox listBoxUrlList;
        private System.Windows.Forms.TextBox textBoxPrintJson;
    }
}

