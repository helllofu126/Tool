using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloFu.BackupTool
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserControlSelectCopyFile : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public UserControlSelectCopyFile()
        {
            InitializeComponent();
        }

        private void UserControlSelectCopyFile_Load(object sender, EventArgs e)
        {
            //初始化定时器timerRefreshPath
            timerRefreshPath.Interval = 100;
            timerRefreshPath.Tick += TimerRefreshPath_Tick;
            timerRefreshPath.Start();

        }

        /// <summary>
        /// 文件路径列表
        /// </summary>
        public List<string> FilePathList
        {
            get
            {
                return crossLineLabelSelectFile.FilePathList;
            }
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        public void ClearFilePathList()
        {
            //清空FilePathList
            crossLineLabelSelectFile?.ClearFilePathList();

            //清空listBoxFilePath
            listBoxFilePath.Items.Clear();
        }

        /// <summary>
        /// 设置lable名称
        /// </summary>
        public string LabelListBoxName
        {
            get
            {
                return labelListBoxName.Text;
            }
            set
            {
                labelListBoxName.Text = value;
            }
        }

        private void TimerRefreshPath_Tick(object sender, EventArgs e)
        {
            //刷新文件路径
            if (crossLineLabelSelectFile.FilePathQueue.Count > 0)
            {
                string filePath = crossLineLabelSelectFile.FilePathQueue.Dequeue();
                if (!listBoxFilePath.Items.Contains(filePath))
                {
                    listBoxFilePath.Items.Add(filePath);
                }
            }
        }

        private void delectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //删除选中的文件路径
            DeleteSelectedFilePath();
        }

        private void listBoxFilePath_KeyDown(object sender, KeyEventArgs e)
        {
            //按Delete键删除选中的文件路径
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedFilePath();
            }
        }

        /// <summary>
        /// 方法：删除选择的文件路径
        /// </summary>
        private void DeleteSelectedFilePath()
        {
            var selectedIndex = listBoxFilePath.SelectedIndex;

            //删除选中的文件
            if (selectedIndex >= 0)
            {
                listBoxFilePath.Items.RemoveAt(selectedIndex);

                //在FilePathList中删除选中的文件
                crossLineLabelSelectFile.FilePathList.RemoveAt(selectedIndex);
            }
        }
    }
}
