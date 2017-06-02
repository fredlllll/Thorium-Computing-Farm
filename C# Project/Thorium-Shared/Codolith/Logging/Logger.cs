using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace Codolith.Logging
{
    public enum LogLevel : byte
    {
        Debug = 40,
        Info = 30,
        Warning = 20,
        Error = 10,
        None = 0
    }

    /// <summary>
    /// Provides functionality for logging.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Metric DateTime format (day.month.year - 24hrs:Mins:Secs.Microsecs)
        /// </summary>
        public const String METRIC_DATETIME_FORMAT = "dd.MM.yy - HH:mm:ss.ffff";

        private StreamWriter streamWriter = null;

        public string HashtagMarker { get; set; } = "#####";

        public Logger(string filename = default(string), bool logCurrentDomainUnhandledExceptions = false)
        {
            MaxLogLevel = LogLevel.Debug;
            DefaultLogLevel = LogLevel.Info;
            LogFirstChanceExceptions = false;
            if(filename == default(string))
            {
                LogFile = new FileInfo(Assembly.GetEntryAssembly().GetName().Name + ".log");
            }
            else
            {
                LogFile = new FileInfo(filename);
            }
            CultureInfo = CultureInfo.InvariantCulture;

            if(logCurrentDomainUnhandledExceptions)
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#if !NET_35
                AppDomain.CurrentDomain.FirstChanceException += new EventHandler<System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException);
#endif
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            }
        }

        /// <summary>
        /// The maximum log level that should be logged
        /// </summary>
        public LogLevel MaxLogLevel { get; set; }

        /// <summary>
        /// The default log level
        /// </summary>
        public LogLevel DefaultLogLevel { get; set; }

        private CultureInfo cultureInfo;
        /// <summary>
        /// The culture used for determining the date time format. The format can always be altered manually afterwards.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get { return cultureInfo; }
            set
            {
                cultureInfo = value;
                DateTimeFormat = cultureInfo.DateTimeFormat.FullDateTimePattern + ".ffff";
                LogCultureInfo();
            }
        }

        /// <summary>
        /// The format used to format the date & time part of a log message.
        /// </summary>
        public String DateTimeFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if every thrown Exception should be Logged.
        /// </summary>
        public static bool LogFirstChanceExceptions
        {
            get;
            set;
        }

        private FileInfo logFile;
        /// <summary>
        /// The currently used log file.
        /// </summary>
        public FileInfo LogFile
        {
            get
            {
                return logFile;
            }
            set
            {
                PrintEndAndClose();
                logFile = value;
            }
        }

        /// <summary>
        /// Logs messages with the given log level.
        /// </summary>
        /// <param name="logLevel">The log level to use</param>
        /// <param name="messages">The messages</param>
        public void Log(LogLevel logLevel, params string[] messages)
        {
            if(streamWriter == null)
            {
                OpenStreamAndPrintStart();
            }
            if(logLevel <= MaxLogLevel)
            {
                foreach(String message in messages)
                {
                    lock(streamWriter)
                    {
                        streamWriter.WriteLine("[" + DateTime.Now.ToString(DateTimeFormat, cultureInfo) + "][" + logLevel.ToString() + "]: " + message);
                    }
                }
                streamWriter.Flush();
            }
        }

        private void LogNoLevel(params string[] messages)
        {
            if(streamWriter == null)
            {
                OpenStreamAndPrintStart();
            }
            foreach(String message in messages)
            {
                lock(streamWriter)
                {
                    streamWriter.WriteLine("[" + DateTime.Now.ToString(DateTimeFormat, cultureInfo) + "]: " + message);
                }
            }
            streamWriter.Flush();
        }

        /// <summary>
        /// Logs messages with the default log level
        /// </summary>
        /// <param name="messages">The messages</param>
        public void Log(params String[] messages)
        {
            Log(DefaultLogLevel, messages);
        }

        /// <summary>
        /// Logs messages with the Debug log level.
        /// </summary>
        /// <param name="messages">The messages</param>
        public void LogDebug(params String[] messages)
        {
            Log(LogLevel.Debug, messages);
        }

        /// <summary>
        /// Logs messages with the Info log level.
        /// </summary>
        /// <param name="messages">The messages</param>
        public void LogInfo(params String[] messages)
        {
            Log(LogLevel.Info, messages);
        }

        /// <summary>
        /// Logs messages with the Warning log level.
        /// </summary>
        /// <param name="messages">The messages</param>
        public void LogWarning(params String[] messages)
        {
            Log(LogLevel.Warning, messages);
        }

        /// <summary>
        /// Logs messages with the Error log level.
        /// </summary>
        /// <param name="messages">The messages</param>
        public void LogError(params String[] messages)
        {
            Log(LogLevel.Error, messages);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogError("Unhandled Error: " + e.ExceptionObject.ToString());
            if(e.IsTerminating)
            {
                LogInfo("Terminating...");
            }
        }
#if !NET_35
        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if(LogFirstChanceExceptions)
            {
                LogWarning("First Chance Exception: " + e.Exception);
            }
        }
#endif

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            PrintEndAndClose();
        }

        private void OpenStreamAndPrintStart()
        {
            if(streamWriter == null)
            {
                FileStream fs = LogFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                fs.Seek(0, SeekOrigin.End);
                //add byte order mark only if at beginning of file
                streamWriter = new StreamWriter(fs, new UTF8Encoding(fs.Position == 0));
                LogNoLevel(HashtagMarker + " Start " + HashtagMarker);
            }
        }

        private void LogCultureInfo()
        {
            if(streamWriter != null)//only log the culture if the file is already open. (if the user wants to change it we would create a log with only this culture info at the beginning)
            {
                LogNoLevel(HashtagMarker + " Culture: " + cultureInfo.EnglishName + " " + HashtagMarker);
            }
        }

        private void PrintEndAndClose()
        {
            if(streamWriter != null)
            {
                LogNoLevel(HashtagMarker + " End " + HashtagMarker);
                streamWriter.Close();
                streamWriter.Dispose();
                streamWriter = null;
            }
        }
    }
}
