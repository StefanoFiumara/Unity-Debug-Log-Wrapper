using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityLogWrapper
{
    internal enum MessageType
    {
        Normal, Error, Warning
    }
    public static class Log
    {
        
        private static string LogFileName { get; set; }
        private static string LogName { get; set; }

        private static bool IsInit { get; set; }

        private static string LogFilePath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), LogFileName); }
        }

        /// <summary>
        /// Initializes the log file with the given properties, the log file will be created in the curent working directory.
        /// </summary>
        /// <param name="fileName">The name of the log file, will append a *.log extension to the name given.</param>
        /// <param name="logName">The title that will be show at the top of your log file.</param>
        [UsedImplicitly]
        public static void Init(string fileName = "", string logName = "")
        {
            if(IsInit) throw new InvalidOperationException("The Log has already been initialized.");

            SetProperties(fileName, logName);

            WriteLogHeader();

            IsInit = true;
        }

        [StringFormatMethod("text"), UsedImplicitly]
        public static void Write(string text, params object[] format)
        {
            Write(MessageType.Normal, format == null ? text : string.Format(text, format));
        }

        [StringFormatMethod("text"), UsedImplicitly]
        public static void Error(string text, params object[] format)
        {
            Write(MessageType.Error, format == null ? text : string.Format(text, format));
        }

        [StringFormatMethod("text"), UsedImplicitly]
        public static void Warning(string text, params object[] format)
        {
            Write(MessageType.Warning, format == null ? text : string.Format(text, format));
        }

        private static void Write(MessageType messageType, string text)
        {
            if (IsInit == false) throw new InvalidOperationException("The log has not yet been initialized!");

            string typePrefix = messageType == MessageType.Normal ? "" : string.Format(" {0}: ", messageType.ToString().ToUpper());

            switch (messageType)
            {
                case MessageType.Normal:
                    Debug.Log(text);
                    break;
                case MessageType.Error:
                    Debug.LogError(text);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("messageType", messageType, null);
            }

            using (var writer = new StreamWriter(LogFilePath, append: true))
            {
                string msg = string.Format("{0:HH:mm:ss} -- {1}{2}", DateTime.Now, typePrefix, text);
                writer.WriteLine(msg);
            }
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
    }
}
