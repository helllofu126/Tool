namespace 环境变量修改
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
            this.components = new System.ComponentModel.Container();
            this.textBoxPrint = new System.Windows.Forms.TextBox();
            this.timerPrint = new System.Windows.Forms.Timer(this.components);
            this.userControlAddPathUser = new 环境变量修改.UserControlAddPath();
            this.userControlAddPathSystem = new 环境变量修改.UserControlAddPath();
            this.SuspendLayout();
            // 
            // textBoxPrint
            // 
            this.textBoxPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPrint.Location = new System.Drawing.Point(738, 12);
            this.textBoxPrint.Multiline = true;
            this.textBoxPrint.Name = "textBoxPrint";
            this.textBoxPrint.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPrint.Size = new System.Drawing.Size(304, 622);
            this.textBoxPrint.TabIndex = 6;
            // 
            // timerPrint
            // 
            this.timerPrint.Tick += new System.EventHandler(this.timerPrint_Tick);
            // 
            // userControlAddPathUser
            // 
            this.userControlAddPathUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlAddPathUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlAddPathUser.Location = new System.Drawing.Point(2, 3);
            this.userControlAddPathUser.Margin = new System.Windows.Forms.Padding(4);
            this.userControlAddPathUser.Name = "userControlAddPathUser";
            this.userControlAddPathUser.PrintDebug = null;
            this.userControlAddPathUser.Size = new System.Drawing.Size(712, 300);
            this.userControlAddPathUser.TabIndex = 7;
            // 
            // userControlAddPathSystem
            // 
            this.userControlAddPathSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlAddPathSystem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlAddPathSystem.Location = new System.Drawing.Point(2, 319);
            this.userControlAddPathSystem.Margin = new System.Windows.Forms.Padding(4);
            this.userControlAddPathSystem.Name = "userControlAddPathSystem";
            this.userControlAddPathSystem.PrintDebug = null;
            this.userControlAddPathSystem.Size = new System.Drawing.Size(712, 317);
            this.userControlAddPathSystem.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1047, 639);
            this.Controls.Add(this.userControlAddPathSystem);
            this.Controls.Add(this.userControlAddPathUser);
            this.Controls.Add(this.textBoxPrint);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormMain";
            this.Text = "环境变量设置";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxPrint;
        private System.Windows.Forms.Timer timerPrint;
        private UserControlAddPath userControlAddPathUser;
        private UserControlAddPath userControlAddPathSystem;
    }
}

