using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 环境变量修改
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timerPrint.Enabled = true;

            //初始化
            userControlAddPathUser.GroupBoxSettingText = "用户变量";
            userControlAddPathUser.PrintDebug = Print;
            userControlAddPathUser.InitEnvironmentVariables(EnvironmentVariableTarget.User);

            userControlAddPathSystem.GroupBoxSettingText = "系统变量";
            userControlAddPathSystem.PrintDebug = Print;
            userControlAddPathSystem.InitEnvironmentVariables(EnvironmentVariableTarget.Machine);

        }

        ConcurrentQueue<string> _printTextQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="text"></param>
        private void Print(string text)
        {

            _printTextQueue.Enqueue($"{DateTime.Now} {text}");
        }

        private void timerPrint_Tick(object sender, EventArgs e)
        {
            string text = string.Empty;
            for (; ; )
            {
                bool ret = _printTextQueue.TryDequeue(out string line);
                if (!ret)
                {
                    if (textBoxPrint.Text.Length > 50000)
                    {
                        textBoxPrint.Text = string.Empty;
                    }
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        textBoxPrint.AppendText(text);
                    }
                    return;
                }
                text += $"{line}\r\n";
            }
        }
    }
}
