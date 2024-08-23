using Ookii.Dialogs.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 环境变量修改
{
    public partial class UserControlAddPath : UserControl
    {
        public UserControlAddPath()
        {
            InitializeComponent();

            //清空combobox
            comboBoxVariableName.Items.Clear();

            //添加path
            comboBoxVariableName.Items.Add("Path");
        }

        volatile IDictionary _environmentVariables;
        EnvironmentVariableTarget? _target;

        /// <summary>
        /// 设置GroupBox的Text
        /// </summary>
        public string GroupBoxSettingText
        {
            set
            {
                groupBoxSetting.Text = value;
            }
        }

        /// <summary>
        /// 打印函数
        /// </summary>
        public D_Print PrintDebug { get; set; }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="text"></param>
        private void Print(string text)
        {
            PrintDebug?.Invoke(text);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="target"></param>
        public void InitEnvironmentVariables(EnvironmentVariableTarget target)
        {
            try
            {
                labelHint.Text = "";

                switch (target)
                {
                case EnvironmentVariableTarget.Process:
                case EnvironmentVariableTarget.User:
                case EnvironmentVariableTarget.Machine:
                    //获取环境变量
                    GetEnvironmentVariables(target);
                    break;
                default:
                    _target = null;
                    _environmentVariables = null;
                    break;
                }

                //设置radiobutton
                radioButtonAll.Checked = true;
            }
            catch (Exception ex)
            {
                //打印异常
                Print($"获取{target}环境变量异常：\r\n{ex.Message}");
            }
        }

        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <param name="target"></param>
        private void GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            try
            {
                _target = target;
                _environmentVariables = Environment.GetEnvironmentVariables(target);

                //开始打印
                Print($"获取{target}环境变量成功");

#if false
                //打印环境变量
                foreach (DictionaryEntry entry in _environmentVariables)
                {
                    Print($"{entry.Key} = {entry.Value}");
                }

                //打印结束
                Print($"打印{target}环境变量结束\r\n");
#endif
            }
            catch (Exception ex)
            {
                //打印异常
                Print($"获取{target}环境变量异常：\r\n{ex.Message}");
            }


        }

        bool _isAddPath = false;

        private void buttonBrowseDirectory_Click(object sender, EventArgs e)
        {
            //创建文件夹对话框,使用VistaFolderBrowserDialog()
            using (var dialog = new VistaFolderBrowserDialog())
            {
                dialog.Description = "请选择文件夹";
                dialog.UseDescriptionForTitle = true;

                //显示对话框
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //设置文件夹路径
                    textBoxVariableValue.Text = dialog.SelectedPath;
                }
            }

        }

        private void comboBoxVariableName_TextChanged(object sender, EventArgs e)
        {
            //获取选中的环境变量为Path
            if (comboBoxVariableName?.Text == "Path")
            {
                buttonAdd.Text = "添加";
                _isAddPath = true;
                return;
            }

            if (buttonAdd.Text != "创建")
            {
                buttonAdd.Text = "创建";
            }
            _isAddPath = false;
        }

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            //创建文件对话框,使用VistaOpenFileDialog()
            using (var dialog = new VistaOpenFileDialog())
            {
                dialog.Title = "请选择文件";
                dialog.Multiselect = false;

                //显示对话框
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //设置文件路径
                    textBoxVariableValue.Text = dialog.FileName;
                }
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //清空combobox
            comboBoxVariableName.Text = "";

            //清空文本框
            textBoxVariableValue.Text = "";

            labelHint.Text = "";
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            //如果_environmentVariables为空，返回
            if (_environmentVariables == null)
            {
                labelHint.Text = "提示:环境变量为空";
                labelHint.ForeColor = Color.Red;
                return;
            }

            //判断名称是否为空
            if (string.IsNullOrWhiteSpace(comboBoxVariableName.Text))
            {
                labelHint.Text = "提示:名称不能为空";
                labelHint.ForeColor = Color.Red;
                return;
            }

            //判断路径是否为空
            if (string.IsNullOrWhiteSpace(textBoxVariableValue.Text))
            {
                labelHint.Text = "提示:路径不能为空";
                labelHint.ForeColor = Color.Red;
                return;
            }

            try
            {
                _mutex.WaitOne();

                //更新环境变量，防止被其他程序修改
                _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);

                labelHint.Text = "提示:正在添加";
                labelHint.ForeColor = Color.Purple;

                //判断是否添加Path
                var isAdd = _isAddPath ? await AddPath() : await CreateEnvironmentVariable();

                //判断是否添加成功
                if (isAdd)
                {
                    labelHint.Text = "提示:添加成功";
                    labelHint.ForeColor = Color.Green;

                    //刷新listview
                    if (radioButtonAll.Checked)
                    {
                        radioButtonAll_CheckedChanged(null, null);
                    }
                    else
                    {
                        radioButtonPath_CheckedChanged(null, null);
                    }
                }
                else
                {
                    labelHint.Text = "提示:添加失败";
                    labelHint.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

        }

        private async Task<bool> CreateEnvironmentVariable()
        {
            //获取combobox的值
            var variableName = comboBoxVariableName.Text;
            var variableValue = textBoxVariableValue.Text;

            //创建环境变量
            try
            {
                //判断是否存在
                if (_environmentVariables.Contains(variableName))
                {
                    MessageBox.Show($"变量名为{variableName}已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                await Task.Run(() =>
                  {
                      //设置环境变量
                      Environment.SetEnvironmentVariable(variableName, variableValue, _target.Value);

                      //更新环境变量
                      _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);

                      //打印
                      Print($"为{_target.Value}创建环境变量成功：{variableName} = {variableValue}");

                  });
                return true;
            }
            catch (Exception ex)
            {
                //打印异常
                Print($"创建环境变量异常：\r\n{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 添加Path
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AddPath()
        {
            var variableValue = textBoxVariableValue.Text;

            //添加信息到Path
            try
            {
                //获取Path
                var path = _environmentVariables["Path"] as string;

                //判断是否存在
                if (path.Contains(variableValue))
                {
                    MessageBox.Show($"路径{variableValue}已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                await Task.Run(() =>
                 {

                     //添加路径
                     path += $"{variableValue};";

                     //设置环境变量
                     Environment.SetEnvironmentVariable("Path", path, _target.Value);

                     //更新环境变量
                     _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);

                     //打印
                     Print($"为{_target.Value}添加Path成功：{variableValue}");

                 });

                return true;
            }
            catch (Exception ex)
            {
                //打印异常
                Print($"添加Path异常：\r\n{ex.Message}");
                return false;
            }
        }

        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //更新环境变量，防止被其他程序修改
                _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);

                //判断是否选中
                if (radioButtonAll.Checked && _environmentVariables != null)
                {
                    //清空listbox
                    listViewShow.Items.Clear();

                    //添加列
                    listViewShow.Columns.Clear();
                    listViewShow.Columns.Add("Variable");
                    listViewShow.Columns.Add("Value");

                    //遍历_environmentVariables，显示到listview
                    foreach (DictionaryEntry entry in _environmentVariables)
                    {
                        listViewShow.Items.Add(new ListViewItem(new[] { entry.Key.ToString(), entry.Value.ToString() }));
                    }

                    //设置自动调整列宽
                    listViewShow.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
            catch { }
        }

        char[] splitChars = new Char[] { ';' };

        private void radioButtonPath_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //更新环境变量，防止被其他程序修改
                _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);

                //判断是否选中
                if (radioButtonPath.Checked && _environmentVariables != null)
                {
                    //清空listbox
                    listViewShow.Items.Clear();
                    listViewShow.Columns.Clear();
                    listViewShow.Columns.Add("Vaule");

                    //获取Path,遍历Path中的路径显示到listbox
                    var path = _environmentVariables["Path"] as string;
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        var paths = path.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var p in paths)
                        {
                            listViewShow.Items.Add(p);
                        }
                    }

                    //设置自动调整列宽
                    listViewShow.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
            catch { }

        }

        /// <summary>
        /// 互斥锁
        /// </summary>
        Mutex _mutex = new Mutex();

        private async void listViewShow_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                _mutex.WaitOne();
                //如果是删除键，删除选中的项
                if (e.KeyCode == Keys.Delete)
                {
                    //判断是否选中
                    if (listViewShow.SelectedItems.Count > 0)
                    {
                        //提示是否删除
                        if (MessageBox.Show("是否删除选中的项", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                        {
                            return;
                        }

                        //显示提示
                        labelHint.Text = "提示:正在删除";
                        labelHint.ForeColor = Color.Purple;

                        //获取选中的项
                        var item = listViewShow.SelectedItems[0];
                        var isRadioPath = radioButtonPath.Checked;

                        await Task.Run(() =>
                          {
                              //判断是否是Path
                              if (isRadioPath)
                              {
                                  //删除Path
                                  var path = _environmentVariables["Path"] as string;
                                  path = path.Replace(item.Text + ";", "");
                                  Environment.SetEnvironmentVariable("Path", path, _target.Value);
                                  _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);
                                  Print($"删除{_target.Value} Path中的值成功：{item.Text}");
                              }
                              else
                              {
                                  //删除环境变量
                                  Environment.SetEnvironmentVariable(item.SubItems[0].Text, "", _target.Value);
                                  _environmentVariables = Environment.GetEnvironmentVariables(_target.Value);
                                  Print($"删除{_target.Value}环境变量成功：{item.SubItems[0].Text} = {item.SubItems[1].Text}");
                              }
                          });

                        //显示提示
                        labelHint.Text = "提示:删除成功";
                        labelHint.ForeColor = Color.Green;

                        //刷新listview
                        if (radioButtonAll.Checked)
                        {
                            radioButtonAll_CheckedChanged(null, null);
                        }
                        else
                        {
                            radioButtonPath_CheckedChanged(null, null);
                        }
                    }
                }
            }
            catch { }
            finally
            {
                _mutex.ReleaseMutex();
            }

        }
    }


    /// <summary>
    /// 委托定义，打印函数
    /// </summary>
    /// <param name="text"></param>
    public delegate void D_Print(string text);
}
