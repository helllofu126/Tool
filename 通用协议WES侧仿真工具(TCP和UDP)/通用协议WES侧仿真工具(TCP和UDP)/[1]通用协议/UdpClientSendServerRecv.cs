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
    public class UdpClientSendServerRecv : ProtocolBase
    {
        public UdpClientSendServerRecv(string remoteIp, int remotePort, int localPort) : base(remoteIp, remotePort, localPort)
        {

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

        //创建udp客户端
        UdpClient _udpClient = null;

        //创建udp服务器
        UdpClient _udpServer = null;

        //析构函数
        ~UdpClientSendServerRecv()
        {
            try
            {
                Dispose(false);
            }
            finally { }
        }

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

        public override bool Start()
        {
            //创建接收线程
            tRecv = new Thread(_Thread_RecvMessage);
            tRecv.Name = "_Thread_RecvMessage";
            tRecv.Priority = ThreadPriority.AboveNormal;
            tRecv.IsBackground = true;
            tRecv.Start();

            //创建发送线程
            tSend = new Thread(_Thread_SendMessage);
            tSend.Name = "_Thread_SendMessage";
            tSend.Priority = ThreadPriority.AboveNormal;
            tSend.IsBackground = true;
            tSend.Start();
            return true;
        }

        private void _Thread_SendMessage()
        {
            byte[] bySend = null;
            Frame frame = null;

            IPEndPoint remoteIpEndPoint = null;

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
                        remoteIpEndPoint = new IPEndPoint(serverIp, RemotePort);
                        _udpClient = new UdpClient();

                        //为了远程主机断开时不会有异常
                        const uint IOC_IN = 0x80000000;
                        int IOC_VENDOR = 0x18000000;
                        int SIO_UDP_CONNRESET = (int)(IOC_IN | IOC_VENDOR | 12);
                        _udpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                        _udpClient.Client.ReceiveBufferSize = 8196 * 1024;
                        _udpClient.Client.ReceiveTimeout = 3000;

                        _udpClient.Connect(remoteIpEndPoint);
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
                    if (bySend.Length == _udpClient.Send(bySend, bySend.Length))
                    {
                        //接收回复
                        Frame response = ReceiveResponse();

                        //打印回复
                        PrintSendMessage($"Receive Response [{frame?.FrameType}] Message:{response?.ToString()}");

                    }

                    frame = null;

                }
                catch (Exception ex)
                {
                    PrintSendMessage($"发生了异常：{ex.Message}");

                    _udpClient?.Close();
                    _udpClient = null;
                }
            }
        }

        IPEndPoint _sendResponseIpEndPoint = null;

        private void _Thread_RecvMessage()
        {
            Frame msg = null;

            FrameBuffer frameBuffer = new FrameBuffer();

            while (false == _isThreadExitEnabled)
            {
                //创建UDP
                IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, LocalPort);
                _udpServer = new UdpClient();
                _udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _udpServer.Client.ReceiveBufferSize = 8196 * 1024;

                //为了远程主机断开时不会有异常
                const uint IOC_IN = 0x80000000;
                int IOC_VENDOR = 0x18000000;
                int SIO_UDP_CONNRESET = (int)(IOC_IN | IOC_VENDOR | 12);
                _udpServer.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

                _udpServer.Client.Bind(localPoint);

                PrintReceiveMessage($"本地端口[{LocalPort}]准备接收来自客户端的信息");

                while (false == _isThreadExitEnabled)
                {
                    try
                    {
                        var byBuff = _udpServer.Receive(ref _sendResponseIpEndPoint);

                        if (null == byBuff)
                        {
                            continue;
                        }

                        //将数据放入缓冲区
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
                        PrintReceiveMessage($"接收数据时发生异常{ex.Message}\r\n{ex.StackTrace}");
                    }
                }

                try
                {
                    //关闭连接
                    _udpServer?.Close();
                    _udpServer = null;
                }
                catch { }

            }
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
                PrintSendMessage($"向发送队列推送数据时发生异常{ex.Message}\r\n{ex.StackTrace}");
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

                //将回复内容转换为字符串
                string sendString = response.ToFrameData();

                //打印回复内容
                PrintReceiveMessage($"Send Response:{response?.ToString()}");

                //将回复内容转换为字节数组
                byte[] bySend = Encoding.UTF8.GetBytes(sendString);

                //发送数据
                if (bySend.Length == _udpServer.Send(bySend, bySend.Length, _sendResponseIpEndPoint))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override Frame ReceiveResponse(object obj = null)
        {
            try
            {

                //将obj转换为IPEndPoint
                IPEndPoint remoteIpEndPoint = null;

                //接收数据
                byte[] byRecv = _udpClient?.Receive(ref remoteIpEndPoint);

                //如果接收数据为空，则返回null
                if (null == byRecv)
                {
                    return null;
                }

                //将数据转换为字符串
                string recvString = Encoding.UTF8.GetString(byRecv);

                //解析数据
                var frame = Frame.FromData(recvString);

                //返回数据
                return frame;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        //是否销毁
        bool _disposing = false;
    }
}


