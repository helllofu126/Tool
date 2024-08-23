using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Wes.Simulator
{
    public class RcsSettings
    {
        /// <summary>
        /// WesRequestRcsApiConfig
        /// </summary>
        public List<WesRequestRcsApiConfig> WesRequestRcsApiConfigList { get; set; } = new List<WesRequestRcsApiConfig>();

    }

    public class WesRequestRcsApiConfig
    {
        /// <summary>
        /// http请求方式
        /// </summary>
        public string HttpType { get; set; }

        /// <summary>
        /// 标签页标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParams { get; set; }
    }

    public class RcsSettingsManager
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="filePath">临时配置参数文件目录</param>
        /// <param name="fileName">临时配置参数文件名称</param>
        public RcsSettingsManager(string filePath, string fileName)
        {
            SettingsFilePath = filePath;
            SettingsFileName = $"{filePath}\\{fileName}";
            RcsSettings = new RcsSettings();

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
        public RcsSettings RcsSettings { get; private set; }

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

                var settings = jsonSerializer.Deserialize<RcsSettings>(jsonReader);
                streamReader.Close();
                jsonReader.Close();
                RcsSettings = settings;
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
                serializer.Serialize(jsonWriter, RcsSettings);

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