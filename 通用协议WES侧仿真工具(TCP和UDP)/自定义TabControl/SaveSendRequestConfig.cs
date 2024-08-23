using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WES.SimulatorTcpUdp
{
    /// <summary>
    /// 发送请求配置
    /// </summary>
    public class SaveSendRequestConfig
    {
        //配置
        public List<SendRequestConfig> SendRequestConfigList { get; set; } = new List<SendRequestConfig>();

    }

    public class SendRequestConfig
    {

        //标题
        public string Title { get; set; }

        //请求类型
        public string RequestType { get; set; }

        //请求类型别名
        public string RequestTypeAlias { get; set; }

        //请求参数
        public string RequestParams { get; set; }
    }


    public class SaveSendRequestConfigManager
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="filePath">临时配置参数文件目录</param>
        /// <param name="fileName">临时配置参数文件名称</param>
        public SaveSendRequestConfigManager(string filePath, string fileName)
        {
            SettingsFilePath = filePath;
            SettingsFileName = $"{filePath}\\{fileName}";
            SaveSendRequestConfig = new SaveSendRequestConfig();

        }

        /// <summary>
        /// 配置文件所在的目录
        /// </summary>
        public string SettingsFilePath { get; }

        /// <summary>
        /// 完整的临时配置参数文件名，对外可访问
        /// </summary>
        public string SettingsFileName { get; }

        /// <summary>
        /// Rcs设置
        /// </summary>
        public SaveSendRequestConfig SaveSendRequestConfig { get; private set; }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Load()
        {
            try
            {
                if (!File.Exists(SettingsFileName)) return false;
                /*以下这些都是固定用法，请参阅Newtonsoft.Json的手册吧*/
                var jsonSerializer = new JsonSerializer();
                var streamReader = new StreamReader(SettingsFileName);
                JsonReader jsonReader = new JsonTextReader(streamReader);

                var settings = jsonSerializer.Deserialize<SaveSendRequestConfig>(jsonReader);
                streamReader.Close();
                jsonReader.Close();
                SaveSendRequestConfig = settings;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Save()
        {
            try
            {
                if (!Directory.Exists(SettingsFilePath))
                {
                    Directory.CreateDirectory(SettingsFilePath);
                }

                /*以下这些都是固定用法，请参阅Newtonsoft.Json的手册吧*/
                var serializer = new JsonSerializer();
                //设置一下缩进的格式，方便阅读
                var streamWriter = new StreamWriter(SettingsFileName);
                var jsonTextWriter = new JsonTextWriter(streamWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                JsonWriter jsonWriter = jsonTextWriter;

                //序列化
                serializer.Serialize(jsonWriter, SaveSendRequestConfig);

                //关闭文件流
                streamWriter.Flush();
                jsonWriter.Flush();
                jsonWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
