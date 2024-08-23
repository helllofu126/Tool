using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Libiao.Common.Web
{
    public delegate void D_WebPrint(string text);

    /// <summary>
    /// 立镖WebService通用类
    /// </summary>
    public class LibiaoWebRequest
    {
#if false
        /// <summary>
        /// Http Post请求
        /// </summary>
        /// <param name="postUrl">请求地址</param>
        /// <param name="postData">请求参数(json格式请求数据时contentType必须指定为application/json)</param>
        /// <param name="result">返回结果</param>
        /// <returns></returns>
        public static bool Post(string postUrl, string postData, out string result)
        {
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(postUrl)) as HttpWebRequest;
                httpWebRequest.Method = "POST";
                //httpWebRequest.KeepAlive = true;
                httpWebRequest.AllowAutoRedirect = true;
                //这个在Post的时候，一定要加上，如果服务器返回错误，还会继续再去请求，不会使用之前的错误数据，做返回数
                httpWebRequest.ServicePoint.Expect100Continue = false;

                httpWebRequest.ContentType = "application/json";

                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentLength = byteArray.Length;

                using (System.IO.Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length); //写入参数
                }

                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                    result = streamReader.ReadToEnd(); //请求返回的数据
                    streamReader.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
#endif

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webPost">请求内容</param>
        /// <param name="webResponse">返回回应内容</param>
        /// <param name="Print">打印方法</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool PostPlus(string url, string webPost, out string webResponse, D_WebPrint Print = null)
        {
            webResponse = string.Empty;

            if (string.IsNullOrWhiteSpace(url))
            {
                Print?.Invoke("Empty URL");
                return false;
            }

            //实例化
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            Print?.Invoke($"Send Request Contents:{url}");
            Print?.Invoke(webPost);

            try
            {
                client.UseDefaultCredentials = true;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("ContentLength", webPost.Length.ToString());
                //client.Headers.Add("MessageID", "1");
                webResponse = client.UploadString(url, "PUT", webPost);
                Print?.Invoke("Receive Response Contents:");
                Print?.Invoke(webResponse);
                return true;
            }
            catch (WebException e)
            {
                Print?.Invoke($"{e.Message}: {e.Response}, {e.Status}");
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Print?.Invoke($"Status Code : {((HttpWebResponse)e.Response).StatusCode}");
                    Print?.Invoke($"Status Description : {((HttpWebResponse)e.Response).StatusDescription}");
                    Print?.Invoke($"Status Description : {(e.Response.ResponseUri)}");
                }
                return false;
            }
            catch (Exception e)
            {
                Print?.Invoke($"{e.Message}: {e.StackTrace},{url},{webPost}");
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }

        public static bool Put(string url, string webPost, out string webResponse, D_WebPrint Print = null)
        {
            return PostPlus(url, webPost, out webResponse, Print);
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webResponse"></param>
        /// <param name="Print"></param>
        /// <returns></returns>
        public static bool Get(string url, out string webResponse, D_WebPrint Print = null)
        {
            webResponse = string.Empty;
            if (string.IsNullOrWhiteSpace(url))
            {
                Print?.Invoke("Empty URL");
                return false;
            }

            //实例化
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            Print?.Invoke($"Request : {url}");
            try
            {
                client.UseDefaultCredentials = true;
                client.Headers.Add("Content-Type", "application/json");
                //Post响应
                webResponse = client.DownloadString(url);
                Print?.Invoke(string.IsNullOrWhiteSpace(webResponse)
                                       ? "Response Empty\r\n"
                                       : $"Response : \r\n{webResponse}");
                return true;
            }
            catch (WebException e)//web异常
            {
                Print?.Invoke($"{e.Message}: {e.Response}, {e.Status}");
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Print?.Invoke($"Status Code : {((HttpWebResponse)e.Response).StatusCode}");
                    Print?.Invoke($"Status Description : {((HttpWebResponse)e.Response).StatusDescription}");
                    Print?.Invoke($"Status Description : {(e.Response.ResponseUri)}\r\n");
                }
                return false;
            }
            catch (Exception e)
            {
                Print?.Invoke($"{e.Message}\r\n{e.StackTrace}\r\n");
                return false;
            }
            finally
            {
                //释放资源
                client.Dispose();
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webPost">请求内容</param>
        /// <param name="webResponse">返回回应内容</param>
        /// <param name="Print">打印方法</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool PostCommon(string url, string webPost, out string webResponse, D_WebPrint Print = null)
        {
            webResponse = string.Empty;
            if (string.IsNullOrWhiteSpace(url))
            {
                Print?.Invoke("Empty URL");
                return false;
            }

            //实例化
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            Print?.Invoke($"Request : {url}\r\n{webPost}");
            try
            {
                client.UseDefaultCredentials = true;
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("ContentLength", webPost.Length.ToString());
                client.Headers.Add("MessageID", "1");
                webResponse = client.UploadString(url, "POST", webPost);//Post响应
                Print?.Invoke(string.IsNullOrWhiteSpace(webResponse)
                    ? "Response Empty\r\n"
                    : $"Response : \r\n{webResponse}");
                return true;
            }
            catch (WebException e)//web异常
            {
                Print?.Invoke($"{e.Message}: {e.Response}, {e.Status}");
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Print?.Invoke($"Status Code : {((HttpWebResponse)e.Response).StatusCode}");
                    Print?.Invoke($"Status Description : {((HttpWebResponse)e.Response).StatusDescription}");
                    Print?.Invoke($"Status Description : {(e.Response.ResponseUri)}\r\n");
                }
                return false;
            }
            catch (Exception e)
            {
                Print?.Invoke($"{e.Message}\r\n{e.StackTrace}\r\n");
                return false;
            }
            finally
            {
                //释放资源
                client.Dispose();
            }
        }

        public static bool Post(string url, string webPost, out string webResponse, D_WebPrint Print = null)
        {
            return PostCommon(url, webPost, out webResponse, Print);
        }
    }
}