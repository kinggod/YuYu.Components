using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace YuYu.Components
{
    /// <summary>
    /// 日志记录机制 by Booth.Lee
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 日志记录文件夹
        /// </summary>
        public static string LogsDirectory { get; set; }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="type">信息类型</param>
        /// <returns></returns>
        public static bool Log(string message, LogType type)
        {
            return _Log(LogsDirectory, message, type);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logsDirectory">日志文件夹</param>
        /// <param name="message">日志信息</param>
        /// <param name="type">信息类型</param>
        /// <returns></returns>
        public static bool Log(string logsDirectory, string message, LogType type)
        {
            return _Log(logsDirectory, message, type);
        }

        private static bool _Log(string logsDirectory, string message, LogType type)
        {
            try
            {
                if (!Directory.Exists(logsDirectory))
                    Directory.CreateDirectory(logsDirectory);
                string filePath = logsDirectory + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
                File.AppendAllText(filePath, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + type + Environment.NewLine + Environment.NewLine + message, Encoding.UTF8);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
