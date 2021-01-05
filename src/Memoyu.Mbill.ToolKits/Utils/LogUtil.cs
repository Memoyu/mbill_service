/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.ToolKits.Utils
*   文件名称 ：LogUtil.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 12:33:21
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace Memoyu.Mbill.ToolKits.Utils
{
    public class LogUtil
    {
        /// <summary>
        /// 记录日常日志
        /// </summary>
        /// <param name="filename">写入日志文件名</param>
        /// <param name="messages">写入信息</param>
        /// <param name="IsHeader">是否加头部分割线</param>
        public static void WriteLog(string filename, string[] messages, bool IsHeader = true)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.File(Path.Combine($"Logs/Serilog/", $"{filename}.log"), rollingInterval: RollingInterval.Infinite, outputTemplate: "{Message}{NewLine}{Exception}")
                .CreateLogger();

            var now = DateTime.Now;
            string logContent = String.Join("\r\n", messages);
            if (IsHeader)
            {
                logContent = (
                   "--------------------------------\r\n" +
                   DateTime.Now + "|\r\n" +
                   String.Join("\r\n", messages) + "\r\n"
                   );
            }

            Log.Information(logContent);
            Log.CloseAndFlush();
        }
    }
}
