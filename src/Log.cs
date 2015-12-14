using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityLogWrapper
{
    public static class Log
    {
        /// <summary>
        /// Gets the name of the log file
        /// </summary>
        public static string LogFileName { get; private set; }
        /// <summary>
        /// Gets the name that appears at the top of the log file
        /// </summary>
        public static string LogName { get; private set; }

        private static bool IsInit { get; set; }

        private static string LogFilePath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), LogFileName); }
        }

        public static void Init(string fileName = "", string logName = "")
        {
            if(IsInit) throw new InvalidOperationException("The Log has already been initialized.");

            SetProperties(fileName, logName);

            WriteLogHeader();

            IsInit = true;
        }

        private static void WriteLogHeader()
        {
            File.WriteAllText(LogFilePath, string.Empty);

            string title = string.Format("------- {0} -------", LogName);
            string header = new string('-', title.Length);

            using (var writer = new StreamWriter(LogFilePath))
            {
                writer.WriteLine(header);
                writer.WriteLine(title);
                writer.WriteLine(header);
                writer.WriteLine("Current Time: {0:MMM dd yyyy HH:mm:ss}", DateTime.Now);
                writer.WriteLine();
            }
        }

        private static void SetProperties(string fileName, string logName)
        {
            LogFileName = fileName;
            LogName = logName;

            if (string.IsNullOrEmpty(LogFileName))
            {
                LogFileName = "UnityLog.log";
            }
            else if (LogFileName.EndsWith(".log") == false)
            {
                LogFileName += ".log";
            }

            if (string.IsNullOrEmpty(LogName))
            {
                LogName = "Unity Log";
            }
        }
        
        private static void Write(string text)
        {
            if(IsInit == false) throw new InvalidOperationException("The log has not yet been initialized!");

            using (var writer = new StreamWriter(LogFilePath, append: true))
            {
                string msg = string.Format("{0:HH:mm:ss} -- {1}", DateTime.Now, text);
                writer.WriteLine(msg);
            }

            Debug.Log(text);
        }

        [StringFormatMethod("text")]
        public static void Write(string text, params object[] format)
        {
            Write(format == null ? text : string.Format(text, format));
        }
    }
}
