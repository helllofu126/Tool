using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WES.SimulatorTcpUdp
{
    public class UdpClientCustom : ProtocolBase
    {

        public UdpClientCustom(string remoteIp, int remotePort) : base(remoteIp, remotePort)
        {

        }

        //析构函数
        ~UdpClientCustom()
        {
            try
            {
                Dispose(false);
            }
            finally { }
        }

        //是否退出线程
        bool _isThreadExitEnabled = false;

        /// <summary>
        /// 创建接收线程
        /// </summary>
        Thread tRecv = null;

        /// <summary>
        /// 创建发送线程
        /// </summary>
        Thread tSend = null;

        [Obsolete]
        public override bool Connect()
        {
            return true;
        }

        public override void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally { }
        }

        //是否销毁
        bool _disposing = false;

        /// <summary>
        /// 销毁
        /// </summary>
        /// <param name="bFreeManagedObjects"></param>
        protected virtual void Dispose(bool bFreeManagedObjects)
        {
            //如果已经销毁，则返回
            if (true == _disposing)
            {
                return;
            }

            //如果线程退出标志为false，则设置为true
            if (false == _isThreadExitEnabled)
            {
                _isThreadExitEnabled = true;
            }

            //如果接收线程不为空，则等待线程退出
            if (null != tRecv)
            {
                if (false == tRecv.Join(1000))
                {
                    tSend.Abort();
                }

                tRecv = null;
            }

            //如果发送线程不为空，则等待线程退出
            if (null != tSend)
            {
                if (false == tSend.Join(1000))
                {
                    tSend.Abort();
                }

                tSend = null;
            }



            //是否需要垃圾回收
            if (bFreeManagedObjects)
            {
                GC.SuppressFinalize(this);
            }

            //设置销毁标志为true
            _disposing = true;
        }

        public override Frame ReceiveResponse(object obj = null)
        {
            return null;
        }

        public override bool SendData(Frame data)
        {
            try
            {
                if (MessageSendQueue.Count >= MESSAGE_SEND_QUEUE_LENGTH)
                {
                    return false;
                }
                MessageSendQueue.Enqueue(data);
                return true;
            }
            catch (Exception ex)
            {
                PrintSendMessage($"向服务端发送队列推送数据时发生异常{ex.Message}\r\n{ex.StackTrace}");
                return false;
            }
        }

        public override bool SendResponse(Frame response)
        {
            try
            {
                //如果回复为空，则返回false
                if (response == null)
                {
                    return false;
                }

                //打印回复内容
                PrintReceiveMessage($"Receive Response:{response.ToString()}");

                //发送数据
                return SendData(response);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// UDP客户端
        /// </summary>
        private UdpClient _udpClient;

        public override bool Start()
        {
            try
            {
                //创建发送线程
                tSend = new Thread(_Thread_UdpSend);
                tSend.IsBackground = true;
                tSend.Priority = ThreadPriority.AboveNormal;
                tSend.Start();

                //创建接收线程
                tRecv = new Thread(_Thread_UdpRecv);
                tRecv.IsBackground = true;
                tRecv.Priority = ThreadPriority.AboveNormal;
                tRecv.Start();

                return true;
            }
            catch { return false; }
        }

        //服务端终端节点
        IPEndPoint _remoteIpEndPoint = null;

        IPEndPoint RemoteIpEndPoint
        {
            get
            {
                return _remoteIpEndPoint;
            }
            set
            {
                //如果节点一样，则返回
                if (true == _remoteIpEndPoint?.Equals(value))
                {
                    return;
                }

                Interlocked.Exchange(ref _remoteIpEndPoint, value);
            }
        }

        //是否连接远程主机
        bool _isConnected = false;

        private void _Thread_UdpSend()
        {
            byte[] bySend = null;
            Frame frame = null;

            DateTime disconnetTime = DateTime.MinValue;

            //如果退出线程标志为false，则一直循环
            while (false == _isThreadExitEnabled)
            {
                try
                {
                    //如果发送客户端为空，则等待100ms,连接
                    if (null == _udpClient)
                    {
                        Thread.Sleep(100);
                        PrintSendMessage($"准备绑定远程主机“{RemoteIp}:{RemotePort}”……");
                        IPAddress serverIp = IPAddress.Parse(RemoteIp);
                        RemoteIpEndPoint = new IPEndPoint(serverIp, RemotePort);
                        _udpClient = new UdpClient();

                        //为了远程主机断开时不会有异常
                        const uint IOC_IN = 0x80000000;
                        int IOC_VENDOR = 0x18000000;
                        int SIO_UDP_CONNRESET = (int)(IOC_IN | IOC_VENDOR | 12);
                        _udpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                        _udpClient.Client.ReceiveBufferSize = 8196 * 1024;

                        _udpClient.Connect(RemoteIpEndPoint);
                        _isConnected = true;
                        PrintSendMessage($"绑定远程主机“{RemoteIp}:{RemotePort}”成功！");
                    }

                    //如果发送数据为空
                    if (null == frame)
                    {
                        //从发送队列中取出数据
                        if (!MessageSendQueue.TryDequeue(out frame))
                        {
                            continue;
                        }
                    }

                    //获取发送的数据
                    string sendString = frame.ToFrameData();

                    //打印发送的数据
                    PrintSendMessage($"Send Message:{frame.ToString()}");

                    //将数据转换为字节数组
                    bySend = Encoding.UTF8.GetBytes(sendString);

                    //发送数据
                    _udpClient.Send(bySend, bySend.Length);
                    frame = null;

                }
                catch (Exception ex)
                {
                    PrintSendMessage($"发生了异常：{ex.Message}");
                    Thread.Sleep(1000);

                    _isConnected = false;
                    _udpClient.Close();
                    _udpClient = null;
                    RemoteIpEndPoint = null;
                }
            }
        }

        private void _Thread_UdpRecv()
        {
            Frame msg = null;

            FrameBuffer frameBuffer = new FrameBuffer();

            IPEndPoint remoteIpEndPoint = null;

            while (false == _isThreadExitEnabled)
            {
                try
                {
                    //如果远程终端节点为空，则等待100ms
                    if (null == RemoteIpEndPoint || !_isConnected)
                    {
                        continue;
                    }

                    //开始接收数据
                    var byBuff = _udpClient.Receive(ref remoteIpEndPoint);

                    //更新远程终端节点
                    RemoteIpEndPoint = remoteIpEndPoint;

                    //将接收到的数据添加到缓冲区
                    frameBuffer.Add(byBuff, byBuff.Length);

                    //尝试解析数据
                    for (; ; )
                    {
                        //如果获取数据帧失败，则退出
                        if (!frameBuffer.TryGetFrame(out byte[] frame))
                        {
                            break;
                        }

                        //将数据帧转换为字符串
                        string strFrame = Encoding.UTF8.GetString(frame, 0, frame.Length);

                        //将字符串转换为数据帧
                        msg = Frame.FromData(strFrame);

                        //如果数据帧不为空，则触发事件
                        if (msg != null)
                        {
                            //打印接收到的数据
                            PrintReceiveMessage($"Receive Message:{msg.ToString()}");

                            //触发接收到消息事件
                            OnRecvMessageEvent(msg);
                        }

                    }

                }
                catch (Exception ex)
                {
                    //打印异常信息
                    PrintReceiveMessage($"WCS接收线程：发生了异常：{ex.Message}");
                }
            }

        }
    }

}
