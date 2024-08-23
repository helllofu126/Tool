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
    public delegate void D_Print(string text);
    public class TcpClientSendServerRecv : ProtocolBase, IDisposable
    {

        public TcpClientSendServerRecv(string name, string ip, int port, int localPort) : base(name, ip, port, localPort)
        {
        }

        public TcpClientSendServerRecv(string ip, int port, int localPort) : base(ip, port, localPort)
        {
        }

        //析构函数
        ~TcpClientSendServerRecv()
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

        //定义发送客户端，用来向RCS发送数据
        TcpClient _tcpcSend = null;

        TcpListener _listener = null;

        //public event RecvMessageEventHandler RecvMessageEvent;

        /// <summary>
        /// 定义发送客户端，用来向RCS发送数据
        /// </summary>
        TcpClient TcpToRcs
        {
            get
            {
                return _tcpcSend;
            }
            set
            {
                Interlocked.Exchange(ref _tcpcSend, value);
            }
        }

        /// <summary>
        /// 接收客户端，用来接收RCS发送的数据
        /// </summary>
        TcpClient _tcpcRecv = null;

        /// <summary>
        /// 接收客户端，用来接收RCS发送的数据
        /// </summary>
        TcpClient TcpFromRcs
        {
            get
            {
                return _tcpcRecv;
            }
            set
            {
                Interlocked.Exchange(ref _tcpcRecv, value);
            }
        }


        /// <summary>
        /// 开启服务
        /// </summary>
        public override bool Start()
        {
            tRecv = new Thread(_Thread_RecvMessage);
            tRecv.Name = "_Thread_RecvMessage";
            tRecv.Priority = ThreadPriority.AboveNormal;
            tRecv.IsBackground = true;
            tRecv.Start();

            tSend = new Thread(_Thread_SendMessage);
            tSend.Name = "_Thread_SendMessage";
            tSend.Priority = ThreadPriority.AboveNormal;
            tSend.IsBackground = true;
            tSend.Start();

            return true;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        [Obsolete("请使用Start()方法,暂时不用了")]
        public override bool Connect()
        {
            //create message send thread
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

            DateTime disconnetTime = DateTime.MinValue;

            //如果退出线程标志为false，则一直循环
            while (false == _isThreadExitEnabled)
            {
                try
                {
                    //如果发送客户端为空，则等待100ms,连接RCS
                    if (null == TcpToRcs)
                    {
                        Thread.Sleep(100);
                        PrintSendMessage($"准备连接服务器“{RemoteIp}:{RemotePort}”……");

                        //连接RCS
                        TcpToRcs = new TcpClient(RemoteIp, RemotePort);

                        PrintSendMessage($"连接服务器“{RemoteIp}:{RemotePort}”成功");
                        IsToRemote = true;

                        //设置接收超时时间
                        TcpToRcs.ReceiveTimeout = 3000;

                        //禁用延时
                        TcpToRcs.NoDelay = true;
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
                    PrintSendMessage($"Send Message:{frame?.ToString()}");

                    //将数据转换为字节数组
                    bySend = Encoding.UTF8.GetBytes(sendString);

                    //发送数据
                    if (bySend.Length == TcpToRcs.Client.Send(bySend))
                    {
                        //接收回复
                        Frame response = ReceiveResponse();
                        string responseMessage = response?.ToString();

                        //打印回复
                        PrintSendMessage($"Receive Response [{frame.FrameType}] Message:{responseMessage}");

                    }

                    frame = null;
                }
                catch (Exception ex)
                {

                    DateTime now = DateTime.Now;
                    if ((now - disconnetTime).TotalSeconds >= 60)
                    {
                        disconnetTime = now;
                        PrintSendMessage($"发生了异常：{ex.Message}");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                    IsToRemote = false;
                    TcpToRcs?.Close();
                    TcpToRcs = null;
                }
            }

        }

        private void _Thread_RecvMessage()
        {
            Frame msg = null;

            byte[] byBuff = new byte[Frame.MaxLength];

            PrintReceiveMessage($"本地端口[{LocalPort}]准备监听来自客户端的连接");
            _listener = new TcpListener(IPAddress.Any, LocalPort);
            _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.Start();

            FrameBuffer frameBuffer = new FrameBuffer();

            while (false == _isThreadExitEnabled)
            {
                Thread.Sleep(100);
                TcpFromRcs = _listener.AcceptTcpClient();
                IPAddress remoteIp = ((IPEndPoint)TcpFromRcs.Client.RemoteEndPoint).Address;
                PrintReceiveMessage($"收到连接请求，客户端的IP地址是[{remoteIp.ToString()}]");
                TcpFromRcs.ReceiveBufferSize = 8196 * 1024;

                while (false == _isThreadExitEnabled)
                {
                    try
                    {
                        //接收数据
                        int nRecv = TcpFromRcs.Client.Receive(byBuff, 0, byBuff.Length, SocketFlags.None);

                        if (0 == nRecv)
                        {
                            PrintReceiveMessage($"接收了0字节的数据");
                            break;
                        }

                        //将接收到的数据添加到缓冲区
                        frameBuffer.Add(byBuff, nRecv);

                        //尝试获取完整的帧
                        for (; ; )
                        {
                            //如果获取数据帧失败，则返回
                            if (!frameBuffer.TryGetFrame(out byte[] frame))
                            {
                                break;
                            }

                            //将字节数组转换为字符串
                            string messageString = Encoding.UTF8.GetString(frame, 0, frame.Length);

                            //将字符串转换为帧
                            msg = Frame.FromData(messageString);

                            //如果帧不为空，则打印帧
                            if (null != msg)
                            {
                                PrintReceiveMessage($"Receive Message:{msg?.ToString()}");

                                //通知外部接收到消息
                                OnRecvMessageEvent(msg);
                                //MessageRecvQueue.Enqueue(msg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //打印异常信息
                        PrintReceiveMessage($"接收线程：发生了异常：{ex.Message}");
                        break;
                    }
                }

                IsToRemote = false;

                try
                {
                    //关闭连接
                    TcpFromRcs?.Close();
                    TcpFromRcs = null;
                }
                catch { }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
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


        /// <summary>
        /// 当接收到来自RCS的信息需要回应时，发送回应信息
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
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
                if (bySend.Length == TcpFromRcs.Client.Send(bySend))
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

        /// <summary>
        /// 接收回应
        /// </summary>
        /// <returns></returns>
        public override Frame ReceiveResponse(object obj = null)
        {
            try
            {
                byte[] byBuff = new byte[8196];
                int nRecv = TcpToRcs.Client.Receive(byBuff, 0, byBuff.Length, SocketFlags.None);
                if (0 == nRecv)
                {
                    return null;
                }
                string messageString = Encoding.UTF8.GetString(byBuff, 0, byBuff.Length);
                Frame msg = Frame.FromData(messageString);
                return msg;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override void Dispose()
        {
            try
            {
                Dispose(true);

                //TCP客户端
                TcpToRcs?.Close();
                TcpToRcs = null;

                //关闭监听
                TcpFromRcs?.Close();
                TcpFromRcs = null;

                //关闭监听
                _listener?.Stop();
                _listener = null;
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
    }
}

