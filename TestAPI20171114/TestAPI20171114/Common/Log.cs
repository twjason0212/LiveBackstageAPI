using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace TestAPI20171114.Common
{
    public class Log
    {

        private static object _ErrorLogLock = new object();
        private static string ErrorLogPath = Thread.GetDomain().BaseDirectory + "\\Log\\Error\\";
        private static void Error(string folder, string title, string text)
        {
            try
            {
                var path = ErrorLogPath + folder + "\\";
                var msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\t" + title + "\t" + text + Environment.NewLine;

                lock (_ErrorLogLock)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    File.AppendAllText(path + DateTime.Now.ToString("yyyyMMdd") + ".log", msg);
                }
            }
            catch { }
        }
        public static void Error(string folder, string title, string text, string paras = " - ")
        {
            text += "\t" + paras;
            Error(folder, title, text);
        }


        private static object _InfoLogLock = new object();
        private static string InfoLogPath = Thread.GetDomain().BaseDirectory + "\\Log\\Info\\";
        private static void Info(string folder, string title, string text)
        {
            try
            {
                var path = InfoLogPath + folder + "\\";
                var msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\t" + title + "\t" + text + Environment.NewLine;

                lock (_InfoLogLock)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    File.AppendAllText(path + DateTime.Now.ToString("yyyyMMddHH") + ".log", msg);
                }
            }
            catch { }
        }
        public static void Info(string folder, string title, string text, string paras = " - ")
        {
            text += "\t" + paras;
            Info(folder, title, text);
        }

    }
}