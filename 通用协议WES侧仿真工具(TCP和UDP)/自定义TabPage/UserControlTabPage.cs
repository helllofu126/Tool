using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wes.Simulator
{
    public sealed partial class UserControlTabPage : UserControl
    {
        public UserControlTabPage()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private JObject _myObject = null;

        /// <summary>
        /// 初始化类信息
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="myObject"></param>
        /// <returns></returns>
        public bool InitializeClass(string requestType, int requestTypeInt, object myObject)
        {
            try
            {
                textBoxRequestType.Text = requestType;

                //如果requestTypeInt大于等于0，则赋值给textBoxRequestTypeInt
                if (requestTypeInt >= 0)
                {
                    textBoxRequestTypeInt.Text = requestTypeInt.ToString();
                }

                if (!string.IsNullOrWhiteSpace(myObject.ToString()))
                {
                    // 解析 JSON 字符串
                    JObject jsonObject = JObject.Parse(myObject.ToString());

                    //将对象赋值给成员变量
                    _myObject = jsonObject;

                    foreach (var filed in jsonObject.Properties())
                    {
                        //添加一个成员
                        AddMember(filed.Name, filed, filed.Value?.ToString());
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                //初始化失败
                MessageBox.Show($"初始化失败{ex.Message}\r\n{ex.StackTrace}");

                return false;
            }
        }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string RequestType { get { return textBoxRequestType.Text; } }

        /// <summary>
        /// 生成的json文本
        /// </summary>
        public string JsonString => textBoxJson.Text;

        /// <summary>
        /// 请求类型的整数值
        /// </summary>
        public int RequestTypeInt
        {
            get
            {
                if (int.TryParse(textBoxRequestTypeInt.Text, out int requestTypeInt))
                {
                    return requestTypeInt;
                }
                return -1;
            }
        }

        /// <summary>
        /// 增加一个成员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddMember(string name, JProperty filed, string value)
        {
            MemberControl memberControl = new MemberControl();
            memberControl.labelName.Text = name;
            memberControl.textBoxValue.Text = value;
            memberControl.textBoxValue.Tag = filed;
            memberControl.Tag = filed;
            memberControl.Width = flowLayoutPanelMembers.Width;
            memberControl.textBoxValue.TextChanged += TextBoxValue_TextChanged;
            flowLayoutPanelMembers.Controls.Add(memberControl);
            return true;
        }

        private void TextBoxValue_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textbox)
            {
                if (textbox.Tag is JProperty field)
                {
                    SetValue(textbox.Text, field); //设置value
                    textBoxJson.Text = ObjectToJsonString(_myObject); //Json输出
                }
            }
        }

        /// <summary>
        /// 设置数值方法
        /// </summary>
        /// <param name="text"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private void SetValue(string text, JProperty property)
        {
            try
            {

                //获取类型
                if (string.IsNullOrWhiteSpace(text))
                {
                    _myObject[property.Name] = null;
                    return;
                }

                if (text.Equals("null", StringComparison.InvariantCultureIgnoreCase))
                {
                    _myObject[property.Name] = null;
                    return;
                }

                //获取类型
                Type type = property.Value.GetType();

                //如果类型是JArray，那么就是数组，则需要特殊处理
                if (type == typeof(JArray))
                {
                    _myObject[property.Name] = JArray.Parse(text);
                    return;
                }

                //如果类型是JObject，那么就是对象，则需要特殊处理
                if (type == typeof(JObject))
                {
                    _myObject[property.Name] = JObject.Parse(text);
                    return;
                }

                _myObject[property.Name] = text.ToString();

                return;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 判断Type是否为null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsIsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// 改变Panel成员Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanelMembers_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in flowLayoutPanelMembers.Controls)
            {
                control.Width = flowLayoutPanelMembers.Width;
            }
        }

        /// <summary>
        /// 对象转Json方法
        /// </summary>
        /// <param name="myObject"></param>
        /// <returns></returns>
        private static string ObjectToJsonString(object myObject)
        {
            if (myObject == null)
            {
                return string.Empty;
            }
            //新建一个JSON的序列化工具
            var serializer = new JsonSerializer();
            //新建一个字符串输出工具
            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, myObject);
            return stringWriter.ToString();
        }

        private void UserControlTabPage_Load(object sender, EventArgs e)
        {
            textBoxJson.Text = ObjectToJsonString(_myObject);
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public MouseEventHandler ButtonSendClickEvent;

        //鼠标点击事件
        private void buttonSend_MouseClick(object sender, MouseEventArgs e)
        {
            //调用发送按钮事件
            ButtonSendClickEvent?.Invoke(this, e);
        }
    }
}