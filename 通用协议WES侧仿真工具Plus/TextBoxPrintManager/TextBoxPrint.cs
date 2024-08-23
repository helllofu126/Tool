using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Libiao.Common
{
    public class TextBoxPrint
    {
        public TextBoxPrint(TextBox textBox, int maxLength)
        {
            _textBox = textBox;
            _timer.Interval = 100;
            _timer.Tick += OnTimerTick;
            _maxLength = maxLength;
        }

        public TextBoxPrint(TextBox textBox) : this(textBox, 65535)
        {
        }

        private readonly int _maxLength = 65535;

        private readonly TextBox _textBox;
        private readonly ConcurrentQueue<string> _textQueue = new ConcurrentQueue<string>();
        private readonly Timer _timer = new Timer();

        public bool Start(string logFile)
        {
            if (!string.IsNullOrWhiteSpace(logFile))
            {
                InitializeLogFile(logFile);
            }
            _timer.Enabled = true;
            return true;
        }

        public bool Start()
        {
            return Start(null);
        }

        private string _logFile = string.Empty;
        private bool _isLogOn = false;


        private bool InitializeLogFile(string logFile)
        {
            _logFile = logFile;
            _isLogOn = true;
            return true;
        }



        public void Print(string text, bool isAutoFormat)
        {
            if (isAutoFormat)
            {
                text = FormatText(text);
            }
            _textQueue.Enqueue(text);//入队
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="text"></param>
        public void Print(string text)
        {
            Print(text, true);
        }

        //timer事件
        private void OnTimerTick(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = null;
            for (; ; )
            {
                if (!_textQueue.TryDequeue(out var line))//出队
                {
                    break;
                }
                if (stringBuilder == null)
                {
                    stringBuilder = new StringBuilder(string.Empty);
                }
                stringBuilder.Append(line);
            }

            if (!(stringBuilder?.Length > 0)) return;
            var text = stringBuilder.ToString();
            if (_textBox.Text.Length > _maxLength)
            {
                _textBox.Text = string.Empty;
            }

            if (_isLogOn)
            {
                try
                {
                    File.AppendAllText(_logFile, text);
                }
                catch
                {
                    return;
                }
            }

            _textBox.AppendText(text);
            _textBox.ScrollToCaret();
        }

        /// <summary>
        /// 文本格式化
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>时间戳+文本</returns>
        private static string FormatText(string text)
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} : {text}\r\n";
        }
    }
}