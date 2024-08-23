using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wes.Simulator;

namespace WES.Simulator
{
    public sealed partial class UserControlTabControl : UserControl
    {
        public UserControlTabControl()
        {
            InitializeComponent();
        }

        //方法：初始化控件
        private void TabControlinit()
        {
            //创建UserControlAddTabPage控件
            UserControlAddTabPage userControlAddTabPage = new UserControlAddTabPage();
            userControlAddTabPage.OkClicked += UserControlAddTabPage_OkClicked;

            //添加FormAddTabPage控件到+标签页
            tabPageAdd.Controls.Add(userControlAddTabPage);
        }

        private void UserControlAddTabPage_OkClicked(object sender, EventArgs e)
        {
            try
            {
                //获取用户输入的标题和URL
                var userControlAddTabPage = sender as UserControlAddTabPage;
                if (userControlAddTabPage != null)
                {
                    AddApiItem(null, userControlAddTabPage.Title, userControlAddTabPage.Url, userControlAddTabPage.RequestParams);

                    //保存配置
                    _wesRequestRcsApiConfigList?.Add(new WesRequestRcsApiConfig
                    {
                        HttpType = "POST",
                        Title = userControlAddTabPage.Title,
                        Url = userControlAddTabPage.Url,
                        RequestParams = userControlAddTabPage.RequestParams
                    });

                    _rcsSettingsManager.Save();

                }
            }
            catch
            {
            }
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
#if true
            var tabControl = sender as TabControl;
            if (e.Index >= tabControl.TabPages.Count)
            {
                return; // 确保索引在有效范围内
            }
            var tabPage = tabControl.TabPages[e.Index];
            var tabRect = tabControl.GetTabRect(e.Index);

            // 检查是否是需要自定义绘制的标签页
            if (tabPage.Text == "+")
            {
                // 设置标题颜色
                var textColor = Color.Blue;

                // 设置字体，剧中，加粗，字号为 15
                var font = new Font("Segoe UI", 20, FontStyle.Bold, GraphicsUnit.Pixel);

                // 绘制背景
                e.Graphics.FillRectangle(new SolidBrush(Color.Pink), tabRect);

                // 绘制标题
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                e.Graphics.DrawString(tabPage.Text, font, new SolidBrush(textColor), tabRect, stringFormat);
            }
            else
            {
                // 使用默认绘制
                e.DrawBackground();
                e.Graphics.DrawString(tabPage.Text, e.Font, new SolidBrush(e.ForeColor), tabRect, new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            }

#endif
        }

        /// <summary>
        /// 鼠标点击发送按钮事件
        /// </summary>
        public MouseEventHandler ButtonSendClickEvent;


        public bool AddApiItem(string httpType, string title, string url, object myObject)
        {
            UserControlTabPage userControlTabPage = new UserControlTabPage();
            userControlTabPage.InitializeClass(httpType, url, myObject);
            userControlTabPage.ButtonSendClickEvent += buttonSend_MouseClick;
            userControlTabPage.RequestMethodChangedEvent += OnRequestMethodChanged;
            userControlTabPage.Tag = title;
            TabPage tabPage = new TabPage();
            tabPage.Text = title;
            tabPage.Controls.Add(userControlTabPage);
            tabControl.TabPages.Insert(tabControl.TabPages.Count - 1, tabPage);
            // 设置当前标签页为选中状态
            //tabControl.SelectedTab = tabPage;

            // 设置鼠标焦点在新标签页的标题上
            //tabControl.Select();
            return true;
        }

        private void OnRequestMethodChanged(object sender, EventArgs e)
        {
            try
            {
                //如果HttpType和记录的不一样，则修改配置
                if (sender is UserControlTabPage userControlTabPage)
                {
                    //获取当前标签页的配置
                    var rcsApiConfig = _wesRequestRcsApiConfigList?.FirstOrDefault(p => p.Title == userControlTabPage.Tag.ToString());

                    //如果配置存在并且不一样，则修改配置
                    if (rcsApiConfig != null && rcsApiConfig.HttpType != userControlTabPage.HttpType)
                    {
                        rcsApiConfig.HttpType = userControlTabPage.HttpType;
                        _rcsSettingsManager.Save();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void buttonSend_MouseClick(object sender, MouseEventArgs e)
        {
            //更新_rcsSettings中的内容
            var userControlTabPage = sender as UserControlTabPage;
            if (userControlTabPage != null)
            {
                var rcsApiConfig = _wesRequestRcsApiConfigList?.FirstOrDefault(p => p.Title == userControlTabPage.Tag.ToString());
                if (rcsApiConfig != null)
                {
                    rcsApiConfig.HttpType = userControlTabPage.HttpType;
                    rcsApiConfig.Url = userControlTabPage.Url;
                    rcsApiConfig.RequestParams = userControlTabPage.JsonString;
                    _rcsSettingsManager.Save();
                }

            }

            // 调用发送按钮事件
            ButtonSendClickEvent?.Invoke(sender, e);
        }


        private List<WesRequestRcsApiConfig> _wesRequestRcsApiConfigList => _rcsSettingsManager?.RcsSettings?.WesRequestRcsApiConfigList;

        private RcsSettingsManager _rcsSettingsManager;

        private void UserControlTabControl_Load(object sender, EventArgs e)
        {
            //初始化TabControl
            TabControlinit();

            //初始化配置管理者
            _rcsSettingsManager = new RcsSettingsManager("Settings", "App WesToRcsSendRequestSettings.config");
            _rcsSettingsManager.Load();

            //遍历请求配置
            foreach (var rcsApiConfig in _wesRequestRcsApiConfigList)
            {
                //添加标签页
                AddApiItem(rcsApiConfig.HttpType, rcsApiConfig.Title, rcsApiConfig.Url, rcsApiConfig.RequestParams);
            }

        }

        private void tabControl_KeyDown(object sender, KeyEventArgs e)
        {
            //如果按下的是delete键
            if (e.KeyCode == Keys.Delete)
            {
                //删除选中的标签页
                RemoveSelectedTabPage();
            }
        }

        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //如果点击的是鼠标右键
                if (e.Button == MouseButtons.Right)
                {
                    //获取当前选中的TabPage
                    TabPage tabPage = tabControl.SelectedTab;

                    // 获取选中 TabPage 的显示区域
                    var selectTabRect = tabControl.GetTabRect(tabControl.SelectedIndex);

                    //判断鼠标是否在选中的TabPage上
                    var isSelecttabPage = selectTabRect.Contains(e.Location);

                    //如果当前鼠标点击的TabPage不是+标签页
                    if (tabPage != null && tabPage.Text != "+" && isSelecttabPage)
                    {
                        //创建右键菜单
                        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

                        //创建菜单项
                        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem($"删除[{tabPage?.Text}]标签页");

                        //为菜单项添加点击事件
                        toolStripMenuItem.Click += ToolStripMenuItem_Click;

                        //将菜单项添加到右键菜单中
                        contextMenuStrip.Items.Add(toolStripMenuItem);

                        //显示右键菜单
                        contextMenuStrip.Show(tabControl, e.Location);
                    }
                }
            }
            catch { }

        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //删除选中的标签页
            RemoveSelectedTabPage();
        }

        /// <summary>
        /// 方法：删除选中的标签页
        /// </summary>
        public void RemoveSelectedTabPage()
        {
            try
            {
                //获取当前选中的TabPage
                TabPage tabPage = tabControl.SelectedTab;

                //如果当前选中的TabPage不是+标签页
                if (tabPage.Text != "+")
                {
                    //弹出提示框，确认是否删除
                    DialogResult result = MessageBox.Show($"确认删除当前选中的【{tabPage?.Text}】标签页吗？", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    //如果用户点击了确认按钮
                    if (result == DialogResult.OK)
                    {

                        var rcsApiConfig = _wesRequestRcsApiConfigList?.FirstOrDefault(p => p.Title == tabPage.Text);
                        if (rcsApiConfig != null)
                        {
                            _wesRequestRcsApiConfigList?.Remove(rcsApiConfig);
                            _rcsSettingsManager.Save();
                        }

                        //移除当前选中的TabPage
                        tabControl.TabPages.Remove(tabPage);
                    }
                }
            }
            catch { }
        }
    }
}
