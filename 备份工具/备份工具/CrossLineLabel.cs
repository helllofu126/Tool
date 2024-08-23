using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloFu.BackupTool
{
    internal class CrossLineLabel : Label
    {
        public CrossLineLabel()
        {
            // 设置控件为重绘模式，这样可以确保十字线条被正确绘制
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.AllowDrop = true;

            FilePathList = new List<string>();
            FilePathQueue = new Queue<string>();
        }

        /// <summary>
        /// 列表：用于存储文件的路径
        /// </summary>
        public List<string> FilePathList { get; set; }

        /// <summary>
        /// 队列：用于输出储存的文件路径
        /// </summary>
        public Queue<string> FilePathQueue { get; private set; }

        /// <summary>
        /// 方法：清空列表
        /// </summary>
        public void ClearFilePathList()
        {
            FilePathList?.Clear();
            FilePathQueue?.Clear();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 获取控件的绘图图面
            Graphics g = e.Graphics;

            // 设置抗锯齿模式，使线条更平滑
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 创建用于绘制线条的笔刷，设置线条颜色和宽度
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                // 设置线条样式为虚线
                //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                //设置四周线条的颜色为灰色
                g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);

                //中心位置写入文字：点击或者拖拽文件到此处
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString("点击或者拖拽文件到此处", this.Font, new SolidBrush(Color.Gray), this.ClientRectangle, sf);
            }
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            try
            {
                base.OnClick(e);

                //创建并配置FolderBrowserDialog
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

                //打开选择对话框，定位到程序运行目录
                folderBrowserDialog.SelectedPath = Application.StartupPath;
                folderBrowserDialog.Description = "选择文件夹";
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取选中的文件夹路径,并将其加入到队列中
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    FilePathList.Add(selectedFolderPath);
                    FilePathQueue.Enqueue(selectedFolderPath);
                }
            }
            catch (Exception ex)
            {
                //打印异常信息
                MessageBox.Show($"点击获取文件夹路径时：{ex.Message}\r\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 拖拽事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            try
            {
                base.OnDragEnter(e);

                //判断拖拽的数据是否是文件
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    //设置拖拽效果为复制
                    e.Effect = DragDropEffects.Copy;

#if true
                    //获取拖拽的文件路径
                    string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

                    //将文件路径加入到队列中
                    foreach (string filePath in filePaths)
                    {
                        FilePathList.Add(filePath);

                        //将文件路径加入到队列中
                        FilePathQueue.Enqueue(filePath);

                        //打印文件路径
                        //Console.WriteLine(filePath);
                    }
#endif
                }
                else
                {
                    //设置拖拽效果为无效
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                //打印异常信息
                MessageBox.Show($"拖拽获取文件夹路径时：{ex.Message}\r\n{ex.StackTrace}");
            }

        }
    }
}
