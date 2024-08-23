using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WES.SimulatorTcpUdp
{
    public class TcpClientCustom : ProtocolBase, IDisposable
    {
        public TcpClientCustom(string ip, int port) : base(ip, port)
        {
        }

        //析构函数
        ~TcpClientCustom()
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
        TcpClient TcpToRemote
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
        /// 
        /// </summary>
        /// <returns></returns>
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
                PrintSendMessage($"向服务端发送队列推送数据时发生异常{ex.Message}\r\n{ex.StackTrace}");
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

                //打印回复内容
                PrintReceiveMessage($"Send Response:{response.ToString()}");

                //发送数据
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
            tSend = new Thread(_Thread_SendMessage);
            tSend.Name = "_Thread_SendMessage";
            tSend.Priority = ThreadPriority.AboveNormal;
            tSend.IsBackground = true;
            tSend.Start();

            return true;
        }

        //是否退出线程
        bool _isThreadExitEnabled = false;

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
                    if (null == TcpToRemote)
                    {
                        Thread.Sleep(100);
                        PrintSendMessage($"准备连接Remote服务器“{RemoteIp}:{RemotePort}”……");

                        //连接RCS
                        TcpToRemote = new TcpClient(RemoteIp, RemotePort);

                        PrintSendMessage($"已连接Remote服务器“{RemoteIp}:{RemotePort}”成功");
                        IsToRemote = true;

                        //设置接收超时时间
                        //TcpToRemote.ReceiveTimeout = 3000;

                        //禁用延时
                        TcpToRemote.NoDelay = true;

                        //连接成功后，创建接收线程，用于接收数据
                        tRecv = new Thread(_Thread_RecvMessage);
                        tRecv.Name = "_Thread_RecvMessage";
                        tRecv.Priority = ThreadPriority.AboveNormal;
                        tRecv.IsBackground = true;
                        tRecv.Start();

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
                    if (bySend.Length == TcpToRemote.Client.Send(bySend))
                    {
#if false
                        //接收回复
                        Frame response = ReceiveResponse();
                        string responseMessage = string.Empty;
                        if (response != null)
                        {
                            //打印回复
                            responseMessage = $"\r\nID:{response?.FrameId}\tType:{response?.FrameType}\tBody:{response?.BodyText}";
                        }

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
                    TcpToRemote?.Close();
                    TcpToRemote = null;
                }
            }

        }

        private void _Thread_RecvMessage()
        {
            Frame msg = null;

            byte[] byBuff = new byte[Frame.MaxLength];

            FrameBuffer frameBuffer = new FrameBuffer();

            //设置接收的数据大小
            TcpToRemote.ReceiveBufferSize = 8196 * 1024;

            while (false == _isThreadExitEnabled)
            {
                try
                {
                    //开始接收数据
                    int iRecv = TcpToRemote.Client.Receive(byBuff);

                    //如果接收数据等于0，则退出
                    if (0 == iRecv)
                    {
                        //打印信息
                        PrintReceiveMessage("接收到0字节数据，退出接收线程");
                        break;
                    }

                    //将接收到的数据添加到缓冲区
                    frameBuffer.Add(byBuff, iRecv);

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
                    break;
                }
            }

            IsToRemote = false;

            try
            {
                //关闭连接
                TcpToRemote?.Close();
                TcpToRemote = null;
            }
            catch { }

        }

        public override void Dispose()
        {
            try
            {
                Dispose(true);

                //关闭连接
                TcpToRemote?.Close();
                TcpToRemote = null;

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
