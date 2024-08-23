using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WES.Simulator
{
    public partial class UserControlAddTabPage : UserControl
    {
        public UserControlAddTabPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 用于存储用户输入的请求参数
        /// </summary>
        public string RequestParams { get { return textBoxRequestParams.Text.Replace('“', '"').Replace('”', '"'); } }

        /// <summary>
        /// 用于存储用户输入的tabPage标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 用于存储用户输入的请求地址
        /// </summary>
        public string Url { get; private set; }


        public event EventHandler OkClicked;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //如果用户输入的tabPage标题为空，则提示用户输入
            if (string.IsNullOrEmpty(textBoxTabPageName.Text))
            {
                MessageBox.Show("请输入tabPage标题");
                return;
            }

            //如果用户输入的请求地址为空，则提示用户输入
            if (string.IsNullOrEmpty(textBoxRequestUrl.Text))
            {
                MessageBox.Show("请输入请求地址");
                return;
            }

            Title = textBoxTabPageName.Text;
            Url = textBoxRequestUrl.Text;
            OkClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 获取或设置TabPage标题输入框的焦点
        /// </summary>
        public void FocusTabPageName()
        {
            textBoxTabPageName.Focus();
        }
    }

    /// <summary>
    /// 用于添加tabPage的窗体的表单
    /// </summary>
    public class FormAddTabPage : Form
    {
        private UserControlAddTabPage userControlAddTabPage;

        public FormAddTabPage()
        {
            //将用户控件添加到窗体中
            userControlAddTabPage = new UserControlAddTabPage { Dock = DockStyle.Fill };
            userControlAddTabPage.OkClicked += MyUserControl_OkClicked;
            Controls.Add(userControlAddTabPage);

            //调整表单大小以适应用户控件
            this.ClientSize = new Size(424, 306);

            //鼠标光标定位到tabPage标题输入框
            this.Load += FormAddTabPage_Load;
        }

        private void FormAddTabPage_Load(object sender, EventArgs e)
        {
            // 使用 BeginInvoke 确保控件已经完全加载
            this.BeginInvoke((Action)(() => userControlAddTabPage.FocusTabPageName()));
        }


        private void MyUserControl_OkClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 获取用户输入的tabPage标题
        /// </summary>
        public string Title => userControlAddTabPage.Title;

        /// <summary>
        /// 获取用户输入的请求地址
        /// </summary>
        public string Url => userControlAddTabPage.Url;

        /// <summary>
        /// 获取用户输入的请求参数
        /// </summary>
        public string RequestParams => userControlAddTabPage.RequestParams;
    }
}
