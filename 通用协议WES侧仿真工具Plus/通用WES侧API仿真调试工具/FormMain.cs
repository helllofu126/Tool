using Libiao.Common;
using Libiao.Common.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wes.Simulator;
using WES.Simulator;

namespace 通用WES侧API仿真调试工具
{
    public partial class FormMain : Form
    {
        public const string Version = "1.0.0";
        public const string ApiVersion = "1.00";
        public FormMain()
        {
            InitializeComponent();
            Text = $"WES SimulatorPlus Ver {Version} (For API {ApiVersion})";
        }

        /// <summary>
        /// 服务端
        /// </summary>
        private LibiaoWebService _libiaoWebService;

        /// <summary>
        /// 打印发送内容
        /// </summary>
        private TextBoxPrint _textBoxSend;

        /// <summary>
        /// 打印接收内容
        /// </summary>
        private TextBoxPrint _textBoxRecv;

        /// <summary>
        /// 本地配置
        /// </summary>
        private WesSettings _wesSettings => _wesSettingsManager?.WesSettings;

        private WEsSettingsManager _wesSettingsManager;

        ConcurrentDictionary<string, JObject> _urlToResponseJsonStringMap = new ConcurrentDictionary<string, JObject>();

        private void buttonAddLocalApi_Click(object sender, EventArgs e)
        {
            //如果服务端为空，则创建服务端
            if (_libiaoWebService == null)
            {
                _libiaoWebService = new LibiaoWebService(_wesSettings.LocalServicePort, _wesSettings.CertName, _textBoxRecv.Print);
                _libiaoWebService.Start();
            }

            //获取输入的url,如果为空则提示用户输入
            string url = textBoxLocalUrl.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入URL");
                return;
            }

            //注册Post请求
            _libiaoWebService.RegisterPostEvent(url, OnResquest);

            //将输入的Json字符串转换为JObject对象
            JObject jObject = null;
            try
            {
                //获取输入的Json字符串
                var inputJsonString = textBoxResponse.Text;

                //如果inputJsonString不为空，则将其转换为JObject对象
                if (!string.IsNullOrEmpty(inputJsonString))
                {
                    //将中文引号替换为英文引号
                    inputJsonString = inputJsonString.Replace('“', '"').Replace('”', '"');

                    //将输入的Json字符串转换为JObject对象
                    jObject = JObject.Parse(inputJsonString);
                }

                //将URL和JObject对象添加到URL映射表中
                _urlToResponseJsonStringMap[url] = jObject;

                //如果ListBox中不存在该URL，将URL添加到ListBox中
                if (!listBoxUrlList.Items.Contains(url))
                {
                    listBoxUrlList.Items.Add(url);
                }

                //保存到本地配置文件
                SaveLocalApi(url, inputJsonString);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"JSON格式错误{ex.Message}");
                return;
            }

        }

        /// <summary>
        /// 保存到本地配置文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inputJsonString"></param>
        private void SaveLocalApi(string url, string inputJsonString)
        {
            //添加到配置文件，如果不存在则添加，如果存在则更新
            var api = _wesSettings.wesApiList.FirstOrDefault(a => a.LocalUrl == url);
            if (api == null)
            {
                _wesSettings.wesApiList.Add(new WesApiConfig { LocalUrl = url, RequestParams = inputJsonString });
            }
            else
            {
                api.RequestParams = inputJsonString;
            }

            //保存配置
            _wesSettingsManager.Save();
        }

        private void OnResquest(string tag, string request, ref string response)
        {
            //打印请求内容
            //_textBoxSend.Print(request);
            response = "  ";

            //如果请求的URL在URL映射表中存在，则返回对应的JSON字符串
            if (_urlToResponseJsonStringMap.ContainsKey(tag))
            {
                response = _urlToResponseJsonStringMap[tag]?.ToString();
            }

        }

        /// <summary>
        /// 加载本地配置文件
        /// </summary>
        private void LoadLocalApi()
        {
            //初始化配置，如果API不为空，则将API添加服务
            if (_wesSettings?.wesApiList?.Count > 0)
            {
                //初始化端口号
                textBoxLocalPort.Text = _wesSettings.LocalServicePort.ToString();
                textBoxLocalPort.Enabled = false;

                //初始化服务端
                _libiaoWebService = new LibiaoWebService(_wesSettings.LocalServicePort, _wesSettings.CertName, _textBoxRecv.Print);
                //初始化ListBox
                foreach (var api in _wesSettings.wesApiList)
                {
                    JObject jObject = null;

                    //如果inputJsonString不为空，则将其转换为JObject对象
                    if (!string.IsNullOrEmpty(api.RequestParams))
                    {
                        //将中文引号替换为英文引号
                        var inputJsonString = api.RequestParams.Replace('“', '"').Replace('”', '"');

                        //将输入的Json字符串转换为JObject对象
                        jObject = JObject.Parse(inputJsonString);
                    }

                    //将URL和JObject对象添加到URL映射表中
                    _urlToResponseJsonStringMap[api.LocalUrl] = jObject;

                    //将URL添加到ListBox中
                    listBoxUrlList.Items.Add(api.LocalUrl);

                    //注册Post请求
                    _libiaoWebService.RegisterPostEvent(api.LocalUrl, OnResquest);
                }

                //启动服务
                _libiaoWebService.Start();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //初始化配置管理者
            _wesSettingsManager = new WEsSettingsManager("Settings", "App WesResponseRcsRequest.config");
            _wesSettingsManager.Load();

            //初始化打印发送文本
            _textBoxSend = new TextBoxPrint(textBoxPost);
            _textBoxSend.Start();

            //初始化打印接收文本
            _textBoxRecv = new TextBoxPrint(textBoxReceived);
            _textBoxRecv.Start();

            //加载本地配置文件
            LoadLocalApi();

            //订阅发送按钮事件
            this.userControlTabControl.ButtonSendClickEvent += ButtonSendClickEvent;

        }

        /// <summary>
        /// 发送按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonSendClickEvent(object sender, MouseEventArgs e)
        {
            try
            {
                //转换成控件内容
                if (sender is UserControlTabPage userControlTabPage)
                {
                    //保存
                    //Save();
                    await Task.Run(() =>
                    {
                        var url = userControlTabPage?.Url;

                        //获取发送的消息
                        var jsonString = GetSendMessage(userControlTabPage?.JsonString);

                        string webResponse = string.Empty;

                        //发送请求
                        switch (userControlTabPage?.HttpType)
                        {
                        case "POST":
                            LibiaoWebRequest.Post(url, jsonString, out webResponse, _textBoxSend.Print);
                            break;
                        case "GET":
                            LibiaoWebRequest.Get(url, out webResponse, _textBoxSend.Print);
                            break;
                        case "PUT":
                            LibiaoWebRequest.Put(url, jsonString, out webResponse, _textBoxSend.Print);
                            break;
                        default:
                            break;
                        }

                    });

                    //打印内容
                    _textBoxSend.Print("\r\n", false);
                }

            }
            catch (Exception ex)
            {
                //打印错误信息
                _textBoxSend.Print($"发送请求时发生错误：{ex.Message}\r\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// 获取发送的消息
        /// 如果需要将发送的内容封装进协议中，可以在这里添加封装
        /// </summary>
        /// <param name="originalJsonString">原始的Json字符串</param>
        /// <returns>封装后的信息</returns>
        private string GetSendMessage(string originalJsonString)
        {
            return originalJsonString;
        }

        private void listBoxUrlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取选中的URL
            string url = listBoxUrlList.SelectedItem?.ToString();

            if (url != null)
            {
                //获取URL对应的JSON字符串
                JObject jObject = _urlToResponseJsonStringMap[url];

                //将JSON字符串显示在文本框中
                var json = jObject?.ToString();
                textBoxPrintJson.Text = json;
                textBoxResponse.Text = json;
                textBoxLocalUrl.Text = url;
            }

        }

        private void listBoxUrlList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //如果用户按下Delete键，则删除选中的URL
                if (e.KeyCode == Keys.Delete)
                {
                    //获取选中的URL
                    string url = listBoxUrlList.SelectedItem?.ToString();

                    //如果URL不为空，则删除URL
                    if (url != null)
                    {
                        _urlToResponseJsonStringMap.TryRemove(url, out _);
                        listBoxUrlList.Items.Remove(url);

                        //去除配置文件中的URL
                        var api = _wesSettings.wesApiList.FirstOrDefault(a => a.LocalUrl == url);
                        if (api != null)
                        {
                            _wesSettings.wesApiList.Remove(api);
                            _wesSettingsManager.Save();
                        }
                    }
                }
            }
            catch { }
        }

        private void buttonSeverStart_Click(object sender, EventArgs e)
        {
            _libiaoWebService?.Start();
        }

        private void textBoxLocalPort_TextChanged(object sender, EventArgs e)
        {
            //如果输入的端口号不是数字，则提示用户
            if (!int.TryParse(textBoxLocalPort.Text, out int port))
            {
                MessageBox.Show("请输入数字");
                return;
            }

            //如果端口号不等于当前端口号，则更新端口号
            if (_wesSettings.LocalServicePort != port)
            {
                _wesSettings.LocalServicePort = port;
            }
        }
    }
}
