namespace WES.SimulatorTcpUdp
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
            this.textBoxPrintJson = new System.Windows.Forms.TextBox();
            this.listBoxUrlList = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.buttonAddLocalApi = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxResponse = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxResponseType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxReceiveType = new System.Windows.Forms.TextBox();
            this.textBoxReceived = new System.Windows.Forms.TextBox();
            this.textBoxSendMessage = new System.Windows.Forms.TextBox();
            this.comboBoxProtocol = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelHostTitle = new System.Windows.Forms.Label();
            this.textBoxRemoteIpAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxResponseTypeInt = new System.Windows.Forms.TextBox();
            this.userControlTabControl = new WES.Simulator.UserControlTabControl();
            this.textBoxReceiveTypeInt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.labelExplain = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxPrintJson
            // 
            this.textBoxPrintJson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrintJson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPrintJson.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPrintJson.Location = new System.Drawing.Point(887, 357);
            this.textBoxPrintJson.Multiline = true;
            this.textBoxPrintJson.Name = "textBoxPrintJson";
            this.textBoxPrintJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPrintJson.Size = new System.Drawing.Size(259, 228);
            this.textBoxPrintJson.TabIndex = 44;
            // 
            // listBoxUrlList
            // 
            this.listBoxUrlList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxUrlList.FormattingEnabled = true;
            this.listBoxUrlList.HorizontalScrollbar = true;
            this.listBoxUrlList.ItemHeight = 15;
            this.listBoxUrlList.Location = new System.Drawing.Point(682, 357);
            this.listBoxUrlList.Name = "listBoxUrlList";
            this.listBoxUrlList.Size = new System.Drawing.Size(199, 229);
            this.listBoxUrlList.TabIndex = 43;
            this.listBoxUrlList.SelectedIndexChanged += new System.EventHandler(this.listBoxUrlList_SelectedIndexChanged);
            this.listBoxUrlList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxUrlList_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 42;
            this.label5.Text = "Local Port ";
            // 
            // textBoxLocalPort
            // 
            this.textBoxLocalPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLocalPort.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLocalPort.Location = new System.Drawing.Point(375, 2);
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.Size = new System.Drawing.Size(79, 26);
            this.textBoxLocalPort.TabIndex = 41;
            this.textBoxLocalPort.Text = "8081";
            this.textBoxLocalPort.TextChanged += new System.EventHandler(this.textBoxLocalPort_TextChanged);
            // 
            // buttonAddLocalApi
            // 
            this.buttonAddLocalApi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddLocalApi.BackColor = System.Drawing.Color.Pink;
            this.buttonAddLocalApi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddLocalApi.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAddLocalApi.Location = new System.Drawing.Point(682, 321);
            this.buttonAddLocalApi.Name = "buttonAddLocalApi";
            this.buttonAddLocalApi.Size = new System.Drawing.Size(461, 31);
            this.buttonAddLocalApi.TabIndex = 40;
            this.buttonAddLocalApi.Text = "添加回复信息";
            this.buttonAddLocalApi.UseVisualStyleBackColor = false;
            this.buttonAddLocalApi.Click += new System.EventHandler(this.buttonAddLocalApi_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(681, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 39;
            this.label2.Text = "Response";
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxResponse.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResponse.Location = new System.Drawing.Point(749, 114);
            this.textBoxResponse.Multiline = true;
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxResponse.Size = new System.Drawing.Size(394, 204);
            this.textBoxResponse.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(679, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 15);
            this.label3.TabIndex = 37;
            this.label3.Text = "ResponseType";
            // 
            // textBoxResponseType
            // 
            this.textBoxResponseType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResponseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxResponseType.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResponseType.Location = new System.Drawing.Point(766, 85);
            this.textBoxResponseType.Name = "textBoxResponseType";
            this.textBoxResponseType.Size = new System.Drawing.Size(257, 26);
            this.textBoxResponseType.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(681, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 35;
            this.label4.Text = "ReceiveType";
            // 
            // textBoxReceiveType
            // 
            this.textBoxReceiveType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceiveType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxReceiveType.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReceiveType.Location = new System.Drawing.Point(766, 53);
            this.textBoxReceiveType.Name = "textBoxReceiveType";
            this.textBoxReceiveType.Size = new System.Drawing.Size(257, 26);
            this.textBoxReceiveType.TabIndex = 34;
            // 
            // textBoxReceived
            // 
            this.textBoxReceived.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceived.Location = new System.Drawing.Point(350, 330);
            this.textBoxReceived.Multiline = true;
            this.textBoxReceived.Name = "textBoxReceived";
            this.textBoxReceived.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxReceived.Size = new System.Drawing.Size(321, 265);
            this.textBoxReceived.TabIndex = 33;
            // 
            // textBoxSendMessage
            // 
            this.textBoxSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSendMessage.Location = new System.Drawing.Point(1, 330);
            this.textBoxSendMessage.Multiline = true;
            this.textBoxSendMessage.Name = "textBoxSendMessage";
            this.textBoxSendMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSendMessage.Size = new System.Drawing.Size(320, 265);
            this.textBoxSendMessage.TabIndex = 32;
            // 
            // comboBoxProtocol
            // 
            this.comboBoxProtocol.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxProtocol.FormattingEnabled = true;
            this.comboBoxProtocol.Items.AddRange(new object[] {
            "TCP",
            "UDP"});
            this.comboBoxProtocol.Location = new System.Drawing.Point(465, 1);
            this.comboBoxProtocol.Name = "comboBoxProtocol";
            this.comboBoxProtocol.Size = new System.Drawing.Size(64, 27);
            this.comboBoxProtocol.TabIndex = 29;
            this.comboBoxProtocol.SelectedIndexChanged += new System.EventHandler(this.comboBoxProtocol_SelectedIndexChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.Pink;
            this.buttonStart.Location = new System.Drawing.Point(535, 1);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(69, 30);
            this.buttonStart.TabIndex = 28;
            this.buttonStart.Text = "开始";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelHostTitle
            // 
            this.labelHostTitle.AutoSize = true;
            this.labelHostTitle.Location = new System.Drawing.Point(10, 11);
            this.labelHostTitle.Name = "labelHostTitle";
            this.labelHostTitle.Size = new System.Drawing.Size(115, 15);
            this.labelHostTitle.TabIndex = 27;
            this.labelHostTitle.Text = "Remote IP Address : ";
            // 
            // textBoxRemoteIpAddress
            // 
            this.textBoxRemoteIpAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRemoteIpAddress.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRemoteIpAddress.Location = new System.Drawing.Point(131, 4);
            this.textBoxRemoteIpAddress.Name = "textBoxRemoteIpAddress";
            this.textBoxRemoteIpAddress.Size = new System.Drawing.Size(165, 26);
            this.textBoxRemoteIpAddress.TabIndex = 26;
            this.textBoxRemoteIpAddress.Text = "127.0.0.1:8511";
            this.textBoxRemoteIpAddress.TextChanged += new System.EventHandler(this.textBoxRemoteIpAddress_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1027, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 21);
            this.label6.TabIndex = 46;
            this.label6.Text = "=";
            // 
            // textBoxResponseTypeInt
            // 
            this.textBoxResponseTypeInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResponseTypeInt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxResponseTypeInt.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxResponseTypeInt.Location = new System.Drawing.Point(1047, 85);
            this.textBoxResponseTypeInt.Name = "textBoxResponseTypeInt";
            this.textBoxResponseTypeInt.Size = new System.Drawing.Size(96, 26);
            this.textBoxResponseTypeInt.TabIndex = 47;
            // 
            // userControlTabControl
            // 
            this.userControlTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlTabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlTabControl.Location = new System.Drawing.Point(1, 36);
            this.userControlTabControl.Margin = new System.Windows.Forms.Padding(4);
            this.userControlTabControl.Name = "userControlTabControl";
            this.userControlTabControl.Size = new System.Drawing.Size(670, 287);
            this.userControlTabControl.TabIndex = 45;
            // 
            // textBoxReceiveTypeInt
            // 
            this.textBoxReceiveTypeInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceiveTypeInt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxReceiveTypeInt.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReceiveTypeInt.Location = new System.Drawing.Point(1047, 53);
            this.textBoxReceiveTypeInt.Name = "textBoxReceiveTypeInt";
            this.textBoxReceiveTypeInt.Size = new System.Drawing.Size(96, 26);
            this.textBoxReceiveTypeInt.TabIndex = 49;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1027, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 21);
            this.label7.TabIndex = 48;
            this.label7.Text = "=";
            // 
            // labelExplain
            // 
            this.labelExplain.AutoSize = true;
            this.labelExplain.Location = new System.Drawing.Point(678, 5);
            this.labelExplain.Name = "labelExplain";
            this.labelExplain.Size = new System.Drawing.Size(46, 15);
            this.labelExplain.TabIndex = 50;
            this.labelExplain.Text = "说明：";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 599);
            this.Controls.Add(this.labelExplain);
            this.Controls.Add(this.textBoxReceiveTypeInt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxResponseTypeInt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.userControlTabControl);
            this.Controls.Add(this.textBoxPrintJson);
            this.Controls.Add(this.listBoxUrlList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxLocalPort);
            this.Controls.Add(this.buttonAddLocalApi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxResponse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxResponseType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxReceiveType);
            this.Controls.Add(this.textBoxReceived);
            this.Controls.Add(this.textBoxSendMessage);
            this.Controls.Add(this.comboBoxProtocol);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.labelHostTitle);
            this.Controls.Add(this.textBoxRemoteIpAddress);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPrintJson;
        private System.Windows.Forms.ListBox listBoxUrlList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.Button buttonAddLocalApi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxResponse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxResponseType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxReceiveType;
        private System.Windows.Forms.TextBox textBoxReceived;
        private System.Windows.Forms.TextBox textBoxSendMessage;
        private System.Windows.Forms.ComboBox comboBoxProtocol;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelHostTitle;
        private System.Windows.Forms.TextBox textBoxRemoteIpAddress;
        private Simulator.UserControlTabControl userControlTabControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxResponseTypeInt;
        private System.Windows.Forms.TextBox textBoxReceiveTypeInt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelExplain;
    }
}

