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
    public class TcpServerCustom : ProtocolBase
    {
        public TcpServerCustom(int localPort) : base(localPort)
        {
        }

        //析构函数
        ~TcpServerCustom()
        {
            try
            {
                Dispose(false);
            }
            finally { }
        }

        //定义客户端
        TcpClient _tcpcSend = null;

        //public event RecvMessageEventHandler RecvMessageEvent;

        /// <summary>
        /// 定义客户端
        /// </summary>
        TcpClient TcpFromRemote
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

        [Obsolete]
        public override bool Connect()
        {
            return true;
        }

        public override Frame ReceiveResponse(object obj = null)
        {
            return null;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
        /// 发送回应
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
                return SendData(response);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建接收线程
        /// </summary>
        Thread tRecv = null;

        /// <summary>
        /// 创建发送线程
        /// </summary>
        Thread tSend = null;

        public override bool Start()
        {
            tRecv = new Thread(_Thread_RecvMessage);
            tRecv.Name = "_Thread_RecvMessage";
            tRecv.Priority = ThreadPriority.AboveNormal;
            tRecv.IsBackground = true;
            tRecv.Start();
            return true;
        }

        //是否退出线程
        bool _isThreadExitEnabled = false;

        TcpListener _listener = null;
        private void _Thread_RecvMessage()
        {
            Frame msg = null;

            byte[] byBuff = new byte[Frame.MaxLength];

            _listener = new TcpListener(IPAddress.Any, LocalPort);
            _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.Start();

            FrameBuffer frameBuffer = new FrameBuffer();

            while (false == _isThreadExitEnabled)
            {
                Thread.Sleep(100);
                PrintReceiveMessage($"WCS本地端口[{LocalPort}]准备监听来自Remote客户端的连接");
                TcpFromRemote = _listener.AcceptTcpClient();
                IPAddress remoteIp = ((IPEndPoint)TcpFromRemote.Client.RemoteEndPoint).Address;
                PrintReceiveMessage($"WCS接受了来自于Remote的连接请求，Remote的IP地址是[{remoteIp.ToString()}]");
                TcpFromRemote.ReceiveBufferSize = 8196 * 1024;

                //创建发送线程
                tSend = new Thread(_Thread_SendMessage);
                tSend.Name = "_Thread_SendMessage";
                tSend.Priority = ThreadPriority.AboveNormal;
                tSend.IsBackground = true;
                tSend.Start();

                while (false == _isThreadExitEnabled)
                {
                    try
                    {
                        //接收数据
                        int iRecv = TcpFromRemote.Client.Receive(byBuff, 0, byBuff.Length, SocketFlags.None);

                        //如果接收到的数据长度为0，则退出
                        if (0 == iRecv)
                        {
                            PrintReceiveMessage($"接收到的数据长度为0");
                            break;
                        }

                        //将接收到的数据放入缓冲区
                        frameBuffer.Add(byBuff, iRecv);

                        //尝试解析数据
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
                        PrintReceiveMessage($"WCS接收线程：发生了异常：{ex.Message}");
                        break;
                    }
                }

                IsToRemote = false;

                try
                {
                    //关闭连接
                    TcpFromRemote?.Close();
                    TcpFromRemote = null;
                }
                catch { }
            }
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
                    if (null == TcpFromRemote)
                    {
                        break;

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
                    if (bySend.Length == TcpFromRemote.Client.Send(bySend))
                    {
#if false
                        //接收回复
                        Frame response = ReceiveResponse();
                        string responseMessage = response?.ToString();

                        //打印回复
                        PrintSendMessage($"Receive Response {frame.FrameType} Message:{responseMessage}");
#endif
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
                    TcpFromRemote?.Close();
                    TcpFromRemote = null;
                }
            }
        }

        public override void Dispose()
        {
            try
            {
                Dispose(true);

                //关闭连接
                TcpFromRemote?.Close();
                TcpFromRemote = null;

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
