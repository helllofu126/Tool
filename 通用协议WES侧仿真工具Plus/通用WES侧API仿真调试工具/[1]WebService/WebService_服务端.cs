using HTTPServerLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Libiao.Common.Web
{
    /// <summary>
    /// 接口请求事件
    /// </summary>
    /// <param name="request">来自客户端的请求信息</param>
    /// <param name="response">返回给客户端的回应信息</param>
    public delegate void PostRequestEventHandler(string request, ref string response);

    /// <summary>
    /// 接口请求事件
    /// </summary>
    /// <param name="request">来自客户端的请求信息</param>
    /// <param name="response">返回给客户端的回应信息</param>
    public delegate void PostRequestTagEventHandler(string Tag, string request, ref string response);

    /// <summary>
    /// 请求地址错误
    /// </summary>
    /// <param name="response"></param>
    public delegate void RequestUrlErrorEventHandler(ref string response);

    /// <summary>
    /// Post请求
    /// </summary>
    public class PostRequest
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PostRequestEventHandler">请求事件</param>
        /// <param name="isPrint">是否打印</param>
        public PostRequest(PostRequestEventHandler PostRequestEventHandler, bool isPrint)
        {
            this.PostRequestEventHandler = PostRequestEventHandler;
            IsPrint = isPrint;
        }

        public PostRequest(PostRequestTagEventHandler PostRequestEventHandler, string tag, bool isPrint)
        {
            this.PostRequestTagEventHandler = PostRequestEventHandler;
            IsPrint = isPrint;
            Tag = tag;
        }

        public PostRequestEventHandler PostRequestEventHandler { get; } = null;
        public PostRequestTagEventHandler PostRequestTagEventHandler { get; } = null;
        public bool IsPrint { get; } = true;

        public string Tag { get; }
    }

    public class LibiaoHttpServer : HttpServer
    {
        private readonly D_WebPrint Print;//打印事件

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="certName">证书</param>
        /// <param name="print">打印</param>
        public LibiaoHttpServer(int port, string certName = null, D_WebPrint print = null) : base("0.0.0.0", port)
        {
            Print = print;
            if (!string.IsNullOrWhiteSpace(certName))//证书
            {
                try
                {
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    foreach (var certificate in store.Certificates)
                    {
                        if (certificate.Subject.Contains(certName))
                        {
                            SetSSL(certificate);
                            break;
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 请求地址错误事件
        /// </summary>
        public event RequestUrlErrorEventHandler RequestUrlErrorEvent;

        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            //获取客户端传递的参数
            //string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());
            string requestType = request.URL.Trim('/').ToLower();

            //构造响应报文
            response.Content_Encoding = "utf-8";
            response.Content_Type = "application/json;charset=UTF-8";

            //读取请求正文
            string requestBody = request.Body;

            //如果该请求是GET，表示是接口里的内容
            if (request.Method.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {

                //构造回应头部格式
                //让外部调用者传入返回给客户端的内容
                string responseBody = string.Empty;

                if (_urlToEventMap.ContainsKey(requestType))
                {
                    PostRequest postRequest = _urlToEventMap[requestType];
                    if (postRequest.IsPrint)
                    {
                        Print?.Invoke($"Received HTTP Request URL : {request.URL}\r\n{requestBody}");
                    }
                    postRequest.PostRequestEventHandler?.Invoke(requestBody, ref responseBody);
                    response.StatusCode = "200";
                    if (postRequest.IsPrint && responseBody?.Length > 0)
                    {
                        Print?.Invoke($"Return HTTP Response:\r\n{responseBody}\r\n");
                    }

                }
                else if (_urlToEventTagMap.ContainsKey(requestType))
                {
                    PostRequest postRequest = _urlToEventTagMap[requestType];
                    if (postRequest.IsPrint)
                    {
                        Print?.Invoke($"Received HTTP Request URL : {request.URL}\r\n{requestBody}");
                    }
                    postRequest.PostRequestTagEventHandler?.Invoke(requestType, requestBody, ref responseBody);
                    response.StatusCode = "200";
                    if (postRequest.IsPrint && responseBody?.Length > 0)
                    {
                        Print?.Invoke($"Return HTTP Response:\r\n{responseBody}\r\n");
                    }
                }
                else
                {
                    Print?.Invoke($"Invalid HTTP Request URL : {request.URL}\r\n{requestBody}");
                    RequestUrlErrorEvent?.Invoke(ref responseBody);//请求地址错误事件响应
                                                                   //地址不存在则返回404
                    response.StatusCode = "404";

                    if (responseBody?.Length > 0)
                    {
                        Print?.Invoke($"Return HTTP Response:\r\n{responseBody}\r\n");
                    }
                }
                //处理外部调用者返回给客户端的内容
                response.Headers.Add("Content-Length", responseBody?.Length.ToString());
                response.SetContent(responseBody);
            }
            else
            {
                //其他类型的请求都返回404错误
                response.StatusCode = "404";
            }

            //发送响应
            response.Send();
        }

        /// <summary>
        /// 存放地址和事件处理的映射
        /// </summary>
        private readonly Dictionary<string, PostRequest> _urlToEventMap = new Dictionary<string, PostRequest>();

        /// <summary>
        /// 存放地址和事件处理的映射
        /// </summary>
        private readonly Dictionary<string, PostRequest> _urlToEventTagMap = new Dictionary<string, PostRequest>();

        /// <summary>
        /// 注册Post事件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="eventHandler">事件处理</param>
        public void RegisterPostEvent(string url, PostRequestEventHandler eventHandler, bool isPrint)
        {
            _urlToEventMap[url.Trim('/').ToLower()] = new PostRequest(eventHandler, isPrint);
        }

        /// <summary>
        /// 注册Post事件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="eventHandler">事件处理</param>
        public void RegisterPostEvent(string url, PostRequestTagEventHandler eventHandler, bool isPrint)
        {
            _urlToEventTagMap[url.Trim('/').ToLower()] = new PostRequest(eventHandler, url, isPrint);
        }


        public void RegisterPostEvent(string url, PostRequestEventHandler eventHandler)
        {
            RegisterPostEvent(url, eventHandler, true);
        }

        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            //获取客户端传递的参数
            //string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());
            string requestType = request.URL.Trim('/').ToLower();

            //构造响应报文
            response.Content_Encoding = "utf-8";
            response.Content_Type = "application/json;charset=UTF-8";

            //解析请求头部发来的messageID
            //string messageID = request.Headers["MessageID"];

            //response.Headers.Add("MessageID", messageID);

            //如果该请求是POST，表示是接口里的内容
            if (request.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                //读取请求正文
                string requestBody = request.Body;

                //构造回应头部格式
                //让外部调用者传入返回给客户端的内容
                string responseBody = string.Empty;

                if (_urlToEventMap.ContainsKey(requestType))
                {
                    PostRequest postRequest = _urlToEventMap[requestType];
                    if (postRequest.IsPrint)
                    {
                        Print?.Invoke($"Received HTTP Request URL : {request.URL}\r\n{requestBody}");
                    }
                    postRequest.PostRequestEventHandler?.Invoke(requestBody, ref responseBody);
                    response.StatusCode = "200";
                    if (postRequest.IsPrint && responseBody?.Length > 0)
                    {
                        Print?.Invoke($"Return HTTP Response:\r\n{responseBody}\r\n");
                    }
                }
                else if (_urlToEventTagMap.ContainsKey(requestType))
                {
                    PostRequest postRequest = _urlToEventTagMap[requestType];
                    if (postRequest.IsPrint)
                    {
                        Print?.Invoke($"Received HTTP Request URL : {request.URL}\r\n{requestBody}");
                    }
                    postRequest.PostRequestTagEventHandler?.Invoke(requestType, requestBody, ref responseBody);
                    response.StatusCode = "200";
                    if (postRequest.IsPrint && responseBody?.Length > 0)
                    {
                        Print?.Invoke($"Return HTTP Response:\r\n{responseBody}\r\n");
                    }
                }
                else
                {
                    Print?.Invoke($"Invalid HTTP Request URL : {request.URL}\r\n{requestBody}");
                    RequestUrlErrorEvent?.Invoke(ref responseBody);//请求地址错误事件响应
                                                                   //地址不存在则返回404
                    response.StatusCode = "404";
                    if (responseBody?.Length > 0)
                    {
                        Print?.Invoke($"Return HTTP Response:\r\n{responseBody}\r\n");
                    }
                }
                //处理外部调用者返回给客户端的内容
                response.Headers.Add("Content-Length", responseBody?.Length.ToString());
                response.SetContent(responseBody);
            }
            else
            {
                //其他类型的请求都返回404错误
                response.StatusCode = "404";
            }
            //发送响应
            response.Send();
        }

        public override void OnDefault(HttpRequest request, HttpResponse response)
        {
        }

        private string ConvertPath(string[] urls)
        {
            string html = string.Empty;
            int length = ServerRoot.Length;
            foreach (var url in urls)
            {
                var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
                html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
            }

            return html;
        }

        private string ListDirectory(string requestDirectory, string requestURL)
        {
            //列举子目录
            var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
            folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
            var foldersList = ConvertPath(folders);

            //列举文件
            var files = Directory.GetFiles(requestDirectory);
            var filesList = ConvertPath(files);

            //构造HTML
            var builder = new StringBuilder();
            builder.Append($"<html><head><title>{requestDirectory}</title></head>");
            builder.Append($"<body><h1>{requestURL}</h1><br/><ul>{filesList}{foldersList}</ul></body></html>");

            return builder.ToString();
        }
    }

    /// <summary>
    /// 立镖WebService通用类
    /// </summary>
    public class LibiaoWebService
    {
        private readonly D_WebPrint _print;

        /// <summary>
        /// 请求地址错误事件
        /// </summary>
        public event RequestUrlErrorEventHandler RequestUrlErrorEvent;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="certName">证书</param>
        /// <param name="Print">打印事件</param>
        public LibiaoWebService(int port = 80, string certName = null, D_WebPrint Print = null)
        {
            _print = Print;
            //提供一个简单的、可通过编程方式控制的 HTTP 协议侦听器。
            _httpServer = new LibiaoHttpServer(port, certName, _print);
            _httpServer.RequestUrlErrorEvent += OnRequestUrlErrorEvent;
        }

        /// <summary>
        /// 请求地址错误事件处理
        /// </summary>
        /// <param name="response"></param>
        private void OnRequestUrlErrorEvent(ref string response)
        {
            RequestUrlErrorEvent?.Invoke(ref response);
        }

        /// <summary>
        /// HTTP监听器
        /// </summary>
        private readonly LibiaoHttpServer _httpServer;

        /*
                /// <summary>
                /// 用于接口的地址名
                /// </summary>
                private string _api;
        */

        /// <summary>
        /// 启动
        /// </summary>
        public bool Start()
        {
            try
            {
                Thread thread = new Thread(_Thread_ClientRequest);
                thread.IsBackground = true;
                thread.Start();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //开启http服务
        private void _Thread_ClientRequest()
        {
            _httpServer.Start();
        }

        //停止http服务
        public void Stop()
        {
            _httpServer.Stop();
        }

        /// <summary>
        /// 注册Post事件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="eventHandler">事件</param>
        public void RegisterPostEvent(string url, PostRequestEventHandler eventHandler)
        {
            RegisterPostEvent(url, eventHandler, true);
        }

        public void RegisterPostEvent(string url, PostRequestEventHandler eventHandler, bool isPrint)
        {
            _httpServer?.RegisterPostEvent(url, eventHandler, isPrint);
        }

        /// <summary>
        /// 注册Post事件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="eventHandler">事件</param>
        public void RegisterPostEvent(string url, PostRequestTagEventHandler eventHandler)
        {
            RegisterPostEvent(url, eventHandler, true);
        }

        public void RegisterPostEvent(string url, PostRequestTagEventHandler eventHandler, bool isPrint)
        {
            _httpServer?.RegisterPostEvent(url, eventHandler, isPrint);
        }

        /// <summary>
        /// 把正文的字符串转为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="bodyString">正文字符串</param>
        /// <returns>成功返回对象类型，失败返回null</returns>
        public static T ConvertStringToClass<T>(string bodyString) where T : class
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                ;
                return JsonConvert.DeserializeObject<T>(bodyString, jsonSerializerSettings);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 把对象转为正文的字符串
        /// </summary>
        /// <param name="bodyClass">待转换的对象</param>
        /// <returns>成功返回字符串，失败返回false</returns>
        public static string ConvertClassToString(object bodyClass)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            };
            ;
            return JsonConvert.SerializeObject(bodyClass, jsonSerializerSettings);
        }
    }
}