using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WES.SimulatorTcpUdp
{
    /// <summary>
    /// 协议基类
    /// </summary>
    public abstract class ProtocolBase : IDisposable
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string RemoteIp { get; set; } = "127.0.0.1";

        /// <summary>
        /// 端口
        /// </summary>
        public int RemotePort { get; set; } = 8511;

        /// <summary>
        /// 本地端口
        /// </summary>
        public int LocalPort { get; set; } = 8081;

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsToRemote { get; set; } = false;

        /// <summary>
        /// 是否开启心跳发送
        /// </summary>
        public bool IsHeartbeatSend { get; set; } = false;

        //委托：接收到消息处理
        public delegate void RecvMessageEventHandler(Frame msg);

        //事件：接收到消息
        public event RecvMessageEventHandler RecvMessageEvent;

        public D_Print PrintSend = null;

        public D_Print PrintReceive = null;


        //构造方法
        public ProtocolBase(string name, string remoteIp, int remotePort, int localPort)
        {
            Name = name;
            RemoteIp = remoteIp;
            RemotePort = remotePort;
            LocalPort = localPort;

            MessageSendQueue = new ConcurrentQueue<Frame>();

        }

        //构造方法
        public ProtocolBase(string remoteIp, int remotePort, int localPort) : this("WCS", remoteIp, remotePort, localPort)
        {
        }

        //构造方法
        public ProtocolBase(string remoteIp, int remotePort) : this("WCS", remoteIp, remotePort, 0)
        {
        }

        //构造方法
        public ProtocolBase(int localPort) : this("WCS", null, 0, localPort)
        {
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        public abstract bool Start();

        /// <summary>
        /// 停止服务
        /// </summary>
        //public abstract void Stop();

        //连接
        [Obsolete("请使用Start()方法,暂时不用了")]
        public abstract bool Connect();

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        public abstract bool SendData(Frame data);

        /// <summary>
        /// 发送回应
        /// </summary>
        /// <param name="data"></param>
        public abstract bool SendResponse(Frame data);

        /// <summary>
        /// 接收回应
        /// </summary>
        public abstract Frame ReceiveResponse(object obj = null);

        /// <summary>
        /// 发送队列
        /// </summary>
        protected ConcurrentQueue<Frame> MessageSendQueue { get; set; }

        /// <summary>
        /// 发送队列长度
        /// </summary>
        protected const int MESSAGE_SEND_QUEUE_LENGTH = 10000;

        private void ClearQueue(ConcurrentQueue<Frame> _messageSendQueue)
        {
            for (; ; )
            {
                if (!_messageSendQueue.TryDequeue(out Frame message))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 触发事件的方法
        /// </summary>
        /// <param name="frame"></param>
        protected virtual void OnRecvMessageEvent(Frame frame)
        {
            RecvMessageEvent?.Invoke(frame);
        }


        /// <summary>
        /// 打印发送消息
        /// </summary>
        /// <param name="text"></param>
        protected void PrintSendMessage(string text)
        {
            PrintSend?.Invoke(text);
        }

        /// <summary>
        /// 打印接收消息
        /// </summary>
        /// <param name="text"></param>
        protected void PrintReceiveMessage(string text)
        {
            PrintReceive?.Invoke(text);
        }

        public abstract void Dispose();

    }
}
