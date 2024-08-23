using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WES.SimulatorTcpUdp
{
    /// <summary>
    /// 报文缓冲区
    /// </summary>
    public class FrameBuffer
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private readonly int _bufferLength;

        /// <summary>
        /// 字段缓冲区
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// 已经使用的缓冲区大小
        /// </summary>
        private int _usedBufferSize = 0;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="bufferLength">缓冲区大小</param>
        public FrameBuffer(int bufferLength = 8196)
        {
            //初始化缓冲区
            _bufferLength = bufferLength;
            _buffer = new byte[_bufferLength];
            Array.Clear(_buffer, 0, _bufferLength);
        }

        /// <summary>
        /// 向缓冲区添加内容
        /// </summary>
        /// <param name="recv">内容</param>
        /// <param name="length">内容长度</param>
        /// <returns></returns>
        public bool Add(byte[] recv, int length)
        {
            //拷贝内容到缓冲区
            Array.Copy(recv, 0, _buffer, _usedBufferSize, length);

            //增加已使用大小
            _usedBufferSize += length;
            return true;
        }

        /// <summary>
        /// 向缓冲区加入内容
        /// </summary>
        /// <param name="recv">接收内容</param>
        /// <param name="length">接收内容长度</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool TryGetFrame(out byte[] frame)
        {
            frame = null;

            //如果缓冲区的第一个字节不是STX（开始标识符），返回false
            if (_buffer[0] != Frame.TAG_STX)
            {
                return false;
            }

            //查找ETX（结束标识符）
            int index = -1;
            for (int i = 1; i < _usedBufferSize; i++)
            {
                if (_buffer[i] == Frame.TAG_ETX)
                {
                    index = i;
                    break;
                }
            }

            //如果没有找到ETX，返回false
            if (index < 0)
            {
                return false;
            }

            //返回报文的长度
            int length = index + 1;

            //返回报文
            frame = new byte[length];

            //把缓冲区里的内容复制到返回报文
            Array.Copy(_buffer, 0, frame, 0, length);

            //减少已使用
            _usedBufferSize -= length;

            //把原来缓冲区的剩余内容取出
            byte[] newBuffer = new byte[_bufferLength];
            Array.Copy(_buffer, length, newBuffer, 0, _usedBufferSize);
            _buffer = newBuffer;
            return true;
        }
    }

    /// <summary>
    /// 报文类型
    /// </summary>
    public enum FrameType
    {
        NONE = 0,
        DEMO = 1,
    }

    /// <summary>
    /// 通讯帧
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// 序号
        /// </summary>
        static int SerialNumber = 0;

        /// <summary>
        /// 起始标识符
        /// </summary>
        public const char TAG_STX = '\x02';

        /// <summary>
        /// 结束标识符
        /// </summary>
        public const char TAG_ETX = '\x03';

        /// <summary>
        /// 数据分隔符
        /// </summary>
        public const char TAG_DLI = '\x1F';

        /// <summary>
        /// 最大长度
        /// </summary>
        public const int MaxLength = 4096;

        /// <summary>
        /// 报文正则表达式
        /// </summary>
        const string CODE_PATTERN = "(?<=\x02)(?<body>[^\x02\x03]+)(?:\x03)";

        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; }

        /// <summary>
        /// 报文序号
        /// </summary>
        public int FrameId { get; private set; } = 0;

        /// <summary>
        /// 报文类型
        /// </summary>
        public string FrameType { get; }

        /// <summary>
        /// 消息内容文本
        /// </summary>
        public string BodyText { get; set; } = string.Empty;

        //public object BodyClass { get; set; } = null;

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        /// 构造一条全新的Message
        /// </summary>
        /// <param name="strSender">发送者</param>
        /// <param name="type">类型</param>
        /// <param name="msgId">信息Id</param>
        /// <param name="body">信息</param>
        private Frame(string strSender, string type, int msgId, string body)
        {
            //发送者
            Sender = strSender;

            //如果消息Id小于0，则自动生成一个消息Id，否则使用传入的消息Id
            if (msgId < 0)
            {
                this.FrameId = Interlocked.Increment(ref SerialNumber);
            }
            else
            {
                this.FrameId = msgId;
            }

            //消息类型
            FrameType = type;

            //消息内容
            BodyText = body;

            //时间戳
            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// 构造一条自己主动发起的消息
        /// </summary>
        /// <param name="type">报文类型</param>
        /// <param name="body">发送内容</param>
        public Frame(string type, string body) : this(Define_Helper.SENDER_WCS, type, -1, body)
        {
        }

        /// <summary>
        /// 构造一条自己主动发起的消息
        /// </summary>
        /// <param name="type">报文类型</param>
        /// <param name="body">发送内容</param>
        public Frame(string type, object body) : this(Define_Helper.SENDER_WCS, type, -1, JsonConvert.SerializeObject(body))
        {
        }

        /// <summary>
        /// 构造一条应答消息
        /// </summary>
        /// <param name="type">报文类型</param>
        /// <param name="body">发送内容</param>
        /// <param name="id">序号</param>
        public Frame(string type, string body, int id) : this(Define_Helper.SENDER_WCS, type, id, body)
        {
        }

        /// <summary>
        /// 构造一条应答消息
        /// </summary>
        /// <param name="type">报文类型</param>
        /// <param name="body">发送内容</param>
        /// <param name="id">序号</param>
        public Frame(string type, object body, int id) : this(Define_Helper.SENDER_WCS, type, id, JsonConvert.SerializeObject(body))
        {
        }

        /// <summary>
        /// 复制一条消息
        /// </summary>
        /// <param name="msg"></param>
        public Frame(Frame msg)
        {
            Sender = msg.Sender;
            FrameId = msg.FrameId;
            FrameType = msg.FrameType;
            BodyText = msg.BodyText;
            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// 转成完整的发送数据帧
        /// </summary>
        /// <returns></returns>
        public string ToFrameData()
        {
            if (int.TryParse(FrameType, out int frameTypeInt))
            {
                //完整的发送数据帧格式：STX+发送者+DLI+序号+DLI+类型+DLI+内容+ETX
                return $"{TAG_STX}{Sender}{TAG_DLI}{FrameId:D010}{TAG_DLI}{frameTypeInt}{TAG_DLI}{BodyText}{TAG_ETX}";
            }

            //完整的发送数据帧格式：STX+发送者+DLI+序号+DLI+类型+DLI+内容+ETX
            return $"{TAG_STX}{Sender}{TAG_DLI}{FrameId:D010}{TAG_DLI}{FrameType}{TAG_DLI}{BodyText}{TAG_ETX}";
        }

        /// <summary>
        /// 转成可以阅读的发送数据帧
        /// </summary>
        /// <returns></returns>
        public string ToFrameText()
        {
            //如果类型字段是数字，则转换为数字，否则直接使用字符串
            if (int.TryParse(FrameType, out int frameTypeInt))
            {
                //完整的发送数据帧格式：[STX]发送者[US]序号[US]类型[US]内容[ETX]
                return $"[STX]{Sender}[US]{FrameId:D010}[US]{frameTypeInt}[US]{BodyText}[ETX]";
            }

            //完整的发送数据帧格式：[STX]发送者[US]序号[US]类型[US]内容[ETX]
            return $"[STX]{Sender}[US]{FrameId:D010}[US]{FrameType}[US]{BodyText}[ETX]";
        }

        /// <summary>
        /// 从报文里解析一条消息
        /// </summary>
        /// <param name="frameText"></param>
        /// <returns></returns>
        public static Frame FromData(string frameText)
        {
            try
            {
                //正则匹配，用于提取报文内容
                Match m = Regex.Match(frameText, CODE_PATTERN);

                //如果没有匹配到内容，返回null
                if (false == m.Success)
                {
                    return null;
                }

                //分割字符串，提取报文内容
                string[] strSplited = m.Groups["body"].Value.Split(TAG_DLI);

                //一条完整的报文至少有3个部分（发件人+id+类型字段），否则返回null
                if (strSplited.Length < 3)
                {
                    return null;
                }

                //如果id无法转换为数字，返回null
                if (false == int.TryParse(strSplited[1], out int id))
                {
                    return null;
                }

#if false
                //如果类型字段无法转换为数字，返回null
                if (false == int.TryParse(strSplited[2], out int typeValue))
                {
                    return null;
                }

                //转换类型字段，构建消息
                FrameType type = (FrameType)typeValue;
#endif

                //如果类型为空，返回null
                var type = strSplited[2];
                if (string.IsNullOrWhiteSpace(strSplited[2]))
                {
                    return null;
                }

                return new Frame(strSplited[0], type, id, strSplited[3]);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"\r\nID:{FrameId}\tType:{FrameType}\tBody:{BodyText}";
        }
    }

    public static class Define_Helper
    {
        //发送者名称：WCS
        public const string SENDER_WCS = "WCS";

        //发送者名称：RCS
        public const string SENDER_RCS = "RCS";

        //时间格式
        public const string DATETIME_FORMAT = "yyyyMMddHHmmssfff";

        //重量格式
        public const string WEIGHT_FORMAT = ".000";

        //心跳间隔秒数
        public const int HEARTBEAT_INTERVAL_SECONDS = 10;
    }
}
