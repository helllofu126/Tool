using Libiao.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;
using Wes.Simulator;

namespace WES.SimulatorTcpUdp
{
    public partial class FormMain : Form
    {
        public const string Version = "1.0.0";
        public const string ApiVersion = "1.00";
        public FormMain()
        {
            InitializeComponent();
            this.Text = $"WES Simulator(TCP/UDP) Ver {Version} (For API {ApiVersion})";

            //初始化comboBox，默认选择TCP
            comboBoxProtocol.SelectedIndex = 0;

            //添加说明到labelExplain：如果需要创建客户端请在RemoteIpAddress处填入连接服务器地址，
            //如果需要创建服务端请在LocalPort处填入本地服务器端口号，
            //如果即需要创建客户端和服务端请两个都填。字体颜色为红色
            labelExplain.Text = "说明：1、如果需要创建客户端请在Remote Ip Address处填入服务器地址\r\n" +
                "             2、如果需要创建服务端请在LocalPort处填入本地服务器端口号\r\n" +
                "             3、如果即需要创建客户端和服务端请两个都填";
            labelExplain.ForeColor = System.Drawing.Color.Red;


        }

        //IP地址配置管理者
        IPAddressConfigManager _ipAddressConfigManager;

        //IP地址配置
        IPAddressConfig ipAddressConfig => _ipAddressConfigManager?.IPAddressSetting;

        //保存发送和回复配置管理者
        SaveSendResponseConfigManager _saveSendResponseConfigManager;

        //获取发送和回复配置
        List<ListBoxSelectShow> _saveResponsetConfig => _saveSendResponseConfigManager?.SaveRequestConfig?.SendResponseConfigList;

        /// <summary>
        /// 打印发送内容
        /// </summary>
        private TextBoxPrint _textBoxSend;

        /// <summary>
        /// 打印接收内容
        /// </summary>
        private TextBoxPrint _textBoxRecv;

        ProtocolBase _protocolBase;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (_protocolBase != null)
            {
                return;
            }

            //如果已经启动，则返回
            buttonStart.Enabled = false;

            var buttonStartText = buttonStart.Text;

            try
            {
                //获取输入框内容
                var inputRemoteIpAddr = textBoxRemoteIpAddress.Text;

                //如果IP地址和端口号为空，则弹窗提示
                if (string.IsNullOrEmpty(inputRemoteIpAddr) && string.IsNullOrEmpty(_inputLocalPort))
                {
                    MessageBox.Show("如果需要创建客户端请在RemoteIpAddress处填入服务器地址\r\n" +
                        "如果需要创建服务端请在LocalPort处填入本地服务器端口号\r\n" +
                        "如果即需要创建客户端和服务端请两个都填");
                    return;
                }
                else if (!string.IsNullOrEmpty(inputRemoteIpAddr) && !string.IsNullOrEmpty(_inputLocalPort))
                {
                    //如果IP地址不为空并且端口号不为空，则创建客户端和服务端
                    CreateClientSendServerRecv();
                }
                else if (!string.IsNullOrEmpty(inputRemoteIpAddr))
                {
                    //如果输入的远程地址不为空，则创建客户端
                    CreateClient();
                }
                else if (!string.IsNullOrEmpty(_inputLocalPort))
                {
                    //如果输入的本地端口不为空，则创建服务端
                    CreateServer();
                }

                if (_protocolBase != null)
                {
                    _protocolBase.RecvMessageEvent += OnRecvMessage;
                    _protocolBase.PrintReceive = _textBoxRecv.Print;
                    _protocolBase.PrintSend = _textBoxSend.Print;
                    _protocolBase.IsHeartbeatSend = _isHeartbeatSend;
                    _protocolBase.Start();
                }

                //保存配置
                SaveIpAddressConfig();

                buttonStart.Enabled = true;


            }
            catch (Exception ex)
            {
                //弹出错误信息
                MessageBox.Show($"启动时出错：{ex.Message}\r\n{ex.StackTrace}");
                buttonStart.Enabled = true;
            }

        }

        /// <summary>
        /// 创建ClientSendServerRecv
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="inputLocalPort"></param>
        private void CreateClientSendServerRecv()
        {
            //如果两个都不为空，则创建客户端和服务端
            if (!string.IsNullOrEmpty(_inputRemoteIp)
                && int.TryParse(_inputRemotePort, out int remotePort)
                && int.TryParse(_inputLocalPort, out int localPort))
            {

                //根据协议类型，创建对应的协议对象
                if (_selectProtocol == "TCP")
                {
                    _protocolBase = new TcpClientSendServerRecv(_inputRemoteIp, remotePort, localPort);
                    return;
                }

                //创建UDP客户端
                _protocolBase = new UdpClientSendServerRecv(_inputRemoteIp, remotePort, localPort);
            }
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="protocol"></param>
        private void CreateClient()
        {
            //如果IP地址不为空，则创建客户端
            if (!string.IsNullOrEmpty(_inputRemoteIp) && int.TryParse(_inputRemotePort, out int remotePort))
            {
                //根据协议类型，创建对应的协议对象
                if (_selectProtocol == "TCP")
                {
                    _protocolBase = new TcpClientCustom(_inputRemoteIp, remotePort);
                    return;
                }

                //创建UDP客户端
                _protocolBase = new UdpClientCustom(_inputRemoteIp, remotePort);
            }
        }

        /// <summary>
        /// 创建服务端
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="inputLocalPort"></param>
        private void CreateServer()
        {
            //如果端口号不为空，则创建服务端
            if (int.TryParse(_inputLocalPort, out int localPort))
            {
                //根据协议类型，创建对应的协议对象
                if (_selectProtocol == "TCP")
                {
                    _protocolBase = new TcpServerCustom(localPort);
                    return;
                }

                //创建UDP服务端
                _protocolBase = new UdpServerCustom(localPort);
            }
        }

        private void OnRecvMessage(Frame msg)
        {
            try
            {
                //获取请求类型
                var requestType = msg.FrameType;
                Frame response = _requestTypeToResponseFramMap.TryGetValue(requestType, out response) ? response : null;

                //发送回复内容
                _protocolBase?.SendResponse(response);

            }
            catch { }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //初始化IP地址配置管理者
            _ipAddressConfigManager = new IPAddressConfigManager("Settings", "IPAddressConfig.json");
            _ipAddressConfigManager.Load();

            //初始化保存发送和回复配置管理者
            _saveSendResponseConfigManager = new SaveSendResponseConfigManager("Settings", "SendResponseConfig.json");
            _saveSendResponseConfigManager.Load();

            //初始化打印发送文本
            _textBoxSend = new TextBoxPrint(textBoxSendMessage);
            _textBoxSend.Start();

            //初始化打印接收文本
            _textBoxRecv = new TextBoxPrint(textBoxReceived);
            _textBoxRecv.Start();

            //订阅发送按钮事件
            this.userControlTabControl.ButtonSendClickEvent += ButtonSendClickEvent;

            //订阅添加标签页事件
            this.userControlTabControl.AddTabPageEvent += OnAddTabPage;

            InitLocalConfig();
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitLocalConfig()
        {
            //获取IP地址配置到文本框中
            textBoxRemoteIpAddress.Text = ipAddressConfig?.RemoteIpAddress;

            //获取本地端口配置到文本框中
            textBoxLocalPort.Text = ipAddressConfig?.LocalPort.ToString();

            //获取协议类型配置到文本框中
            comboBoxProtocol.Text = ipAddressConfig?.ProtocolType;

            //T添加到ListBox中
            foreach (var responsetConfig in _saveResponsetConfig)
            {
                //获取回复类型
                string responseType = responsetConfig.ResponseType;
                //如果回复类型是数字，则转换为字符串
                if (int.TryParse(responsetConfig.ResponseTypeInt, out int responseTypeInt))
                {
                    responseType = responseTypeInt.ToString();
                }

                listBoxUrlList.Items.Add(responsetConfig.ResponseType);
                _listBoxSelectToShowMap[responsetConfig.ResponseType] = responsetConfig;

                //将回复类型和回复内容添加到字典中
                _requestTypeToResponseFramMap[responsetConfig.ReceiveType] = new Frame(responseType, responsetConfig.Response);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveIpAddressConfig()
        {
            //保存IP地址配置
            ipAddressConfig.RemoteIpAddress = textBoxRemoteIpAddress?.Text;
            ipAddressConfig.LocalPort = _inputLocalPort;
            ipAddressConfig.ProtocolType = comboBoxProtocol.Text;

            //保存配置
            _ipAddressConfigManager?.Save();
        }

        /// <summary>
        /// 添加标签页
        /// </summary>
        /// <param name="title"></param>
        /// <param name="requestType"></param>
        /// <param name="requestTypeInt"></param>
        /// <param name="myObject"></param>
        private void OnAddTabPage(string title, string requestType, int requestTypeInt, object myObject)
        {

        }

        private void ButtonSendClickEvent(object sender, MouseEventArgs e)
        {
            //获取发送的内容
            var frame = GetSendContent(sender);

            //frame不为空，则发送
            if (frame != null)
            {
                _protocolBase?.SendData(frame);
            }


        }

        /// <summary>
        /// 获取发送的内容
        /// </summary>
        /// <param name="userControlTabPage"></param>
        /// <returns></returns>
        private Frame GetSendContent(object sender)
        {
            try
            {
                //转换成控件内容
                if (sender is UserControlTabPage userControlTabPage)
                {
                    //获取控件上的请求类型，和请求内容
                    string frameType = userControlTabPage?.RequestType;
                    string jsonString = userControlTabPage?.JsonString;
                    int? frameTypeInt = userControlTabPage?.RequestTypeInt;
                    if (frameTypeInt >= 0)
                    {
                        return new Frame(frameTypeInt.ToString(), jsonString);
                    }
                    return new Frame(frameType, jsonString);
                }
                return null;
            }
            catch (Exception ex)
            {
                //弹出错误信息
                MessageBox.Show($"获取发送内容时出错：{ex.Message}\r\n{ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// 创建字典，用于存储请求类型和回复类型
        /// </summary>
        ConcurrentDictionary<string, Frame> _requestTypeToResponseFramMap = new ConcurrentDictionary<string, Frame>();

        /// <summary>
        /// 字典，用于存储ListBox中的显示内容与选中显示的内容
        /// </summary>
        ConcurrentDictionary<string, ListBoxSelectShow> _listBoxSelectToShowMap = new ConcurrentDictionary<string, ListBoxSelectShow>();

        private void buttonAddLocalApi_Click(object sender, EventArgs e)
        {
            //获取请求类型
            string receiveType = textBoxReceiveType.Text;
            //如果请求类型为空，则弹窗提示
            if (string.IsNullOrEmpty(receiveType))
            {
                MessageBox.Show("请输入请求类型");
                return;
            }

            //如果请求类型是数字，则转换为字符串
            var inputReceiveTypeInt = textBoxReceiveTypeInt.Text;
            if (int.TryParse(inputReceiveTypeInt, out int receiveTypeInt))
            {
                receiveType = receiveTypeInt.ToString();
            }

            //获取回复类型
            string responseType = textBoxResponseType.Text;
            if (string.IsNullOrEmpty(responseType))
            {
                //如果回复类型为空，则弹窗提示
                MessageBox.Show("请输入回复类型");
                return;
            }

            //如果回复类型是数字，则转换为字符串
            var inputResponseTypeInt = textBoxResponseTypeInt.Text;
            if (int.TryParse(inputResponseTypeInt, out int responseTypeInt))
            {
                responseType = responseTypeInt.ToString();
            }

            //获取回复内容
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
                _requestTypeToResponseFramMap[receiveType] = new Frame(responseType, jObject);

                //如果ListBox中不存在该URL，将URL添加到ListBox中
                var addToListBox = textBoxResponseType.Text;
                if (!listBoxUrlList.Items.Contains(addToListBox))
                {
                    listBoxUrlList.Items.Add(addToListBox);
                }

                //将URL和JObject对象添加到ListBox映射表中
                var addResponseMessage = new ListBoxSelectShow
                {
                    ReceiveType = textBoxReceiveType.Text,
                    ReceiveTypeInt = inputReceiveTypeInt,
                    ResponseType = addToListBox,
                    ResponseTypeInt = inputResponseTypeInt,
                    Response = jObject
                };

                //用于显示到ListBox中的内容
                _listBoxSelectToShowMap[addToListBox] = addResponseMessage;

                //添加到保存配置中
                _saveResponsetConfig.Add(addResponseMessage);

                //保存配置
                _saveSendResponseConfigManager.Save();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"JSON格式错误{ex.Message}");
                return;
            }
        }

        private void listBoxUrlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取选中的回复类型
            var selectItem = listBoxUrlList.SelectedItem?.ToString();
            if (selectItem != null)
            {
                //获取选中的回复类型
                var selectShow = _listBoxSelectToShowMap[selectItem];
                if (selectShow != null)
                {
                    //将选中的回复类型显示到文本框中
                    textBoxReceiveType.Text = selectShow.ReceiveType;
                    textBoxReceiveTypeInt.Text = selectShow.ReceiveTypeInt;
                    textBoxResponseType.Text = selectShow.ResponseType;
                    textBoxResponseTypeInt.Text = selectShow.ResponseTypeInt;

                    var response = selectShow.Response?.ToString();
                    textBoxResponse.Text = response;
                    textBoxPrintJson.Text = response;
                }
            }
        }

        string _inputRemoteIp = "127.0.0.1";
        string _inputRemotePort = "8511";

        private void textBoxRemoteIpAddress_TextChanged(object sender, EventArgs e)
        {
            //获取输入的IP地址
            var inputRemoteIp = ((TextBox)sender).Text;

            //如果输入的IP地址为空，则返回
            if (string.IsNullOrEmpty(inputRemoteIp))
            {
                return;
            }

            //解析IP地址和端口号，用：分割
            var remoteIpAndPort = inputRemoteIp?.Split(':');

            //如果IP地址和端口号都不为空，则更新IP地址和端口号
            if (remoteIpAndPort.Length == 2)
            {
                _inputRemoteIp = remoteIpAndPort[0];
                _inputRemotePort = remoteIpAndPort[1];
            }
        }

        string _inputLocalPort = "8081";

        private void textBoxLocalPort_TextChanged(object sender, EventArgs e)
        {
            //获取输入的端口号
            _inputLocalPort = ((TextBox)sender).Text;

        }

        string _selectProtocol = "TCP";

        private void comboBoxProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取选择的协议类型
            _selectProtocol = ((ComboBox)sender).SelectedItem.ToString();
        }

        bool _isHeartbeatSend = false;
        private void checkBoxIsHeartbeat_CheckedChanged(object sender, EventArgs e)
        {
            //获取是否发送心跳
            _isHeartbeatSend = ((CheckBox)sender).Checked;

            if (_protocolBase != null)
            {
                _protocolBase.IsHeartbeatSend = _isHeartbeatSend;
            }
        }

        private void listBoxUrlList_KeyDown(object sender, KeyEventArgs e)
        {
            //如果按下Delete键，则删除选中的回复类型
            if (e.KeyCode == Keys.Delete)
            {
                //获取选中的回复类型
                var selectItem = listBoxUrlList.SelectedItem?.ToString();
                if (selectItem != null)
                {
                    //删除选中的回复类型
                    listBoxUrlList.Items.Remove(selectItem);

                    //删除选中的回复类型
                    if (_listBoxSelectToShowMap.TryRemove(selectItem, out var showConfig))
                    {
                        //删除选中的回复类型
                        _requestTypeToResponseFramMap?.TryRemove(showConfig.ReceiveType, out _);

                        //删除选中的回复类型
                        _saveResponsetConfig.Remove(showConfig);
                    }
                    //保存配置
                    _saveSendResponseConfigManager.Save();
                }
            }
        }
    }

    public class ListBoxSelectShow
    {
        public string ReceiveType { get; set; }
        public string ReceiveTypeInt { get; set; }
        public string ResponseType { get; set; }

        public string ResponseTypeInt { get; set; }
        public JObject Response { get; set; }
    }
}
