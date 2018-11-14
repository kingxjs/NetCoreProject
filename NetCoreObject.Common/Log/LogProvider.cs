using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NetCoreObject.Common
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogProvider
    {
        #region Member
        /// <summary>
        /// 日志文件的物理路径
        /// </summary>
        private static readonly string File = FileHelper.MapPath("/Logs/");
        /// <summary>
        /// 为保持互斥线程安全。
        /// </summary>
        private static readonly object Mutex = new object();


        #endregion

        /// <summary>
        /// Logs an error to the log file.
        /// </summary>
        /// <param name="origin">The origin of the error</param>
        /// <param name="source">来源</param>
        /// <param name="message">The message</param>
        /// <param name="details">Optional error details</param>
        public static void Error(string origin, string source, string message, Exception details = null)
        {
            lock (Mutex)
            {
                var path = File + DateTime.Now.ToString("yyyyMM");
                if (!FileHelper.IsExistDirectory(path))
                {
                    FileHelper.CreateFolder(path);
                }
                FileStream fs = new FileStream(path + "/log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                using (var writer = new StreamWriter(fs))
                {
                    writer.WriteLine($"***********Begin***********");
                    writer.WriteLine(
                        $"ERROR [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Origin [{origin}] Message [{message}] Source [{source}]");
                    writer.WriteLine($"***********End***********\r\n\r\n");
                    if (details != null)
                        writer.WriteLine(details.Message);
                }
                fs.Dispose();
            }
        }
        public static void Error(string origin, Exception details = null)
        {
            lock (Mutex)
            {
                var path = File + DateTime.Now.ToString("yyyyMM");
                if (!FileHelper.IsExistDirectory(path))
                {
                    FileHelper.CreateFolder(path);
                }
                FileStream fs = new FileStream(path + "/log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                using (var writer = new StreamWriter(fs))
                {
                    writer.WriteLine($"***********Begin***********");
                    
                    if (details != null)
                        writer.WriteLine(
                        $"ERROR [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Origin [{origin}] Message [{details.Message}] Source [{details.StackTrace}]");
                    else
                    {
                        writer.WriteLine(
                        $"ERROR [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Origin [{origin}] Message [] Source []");
                    }
                    writer.WriteLine($"***********End***********\r\n\r\n");
                }
                fs.Dispose();
            }
        }
    }
}
