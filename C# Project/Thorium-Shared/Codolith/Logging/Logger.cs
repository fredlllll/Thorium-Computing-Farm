using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Reflection;
using Codolith.Logging.Listeners;

namespace Codolith.Logging
{
    public enum LogLevel : byte
    {
        Always = 250,
        Debug = 200,
        Info = 150,
        Warning = 100,
        Error = 50,
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
        public const string METRIC_DATETIME_FORMAT = "dd.MM.yy - HH:mm:ss.ffff";

        private List<ILogListener> listeners = new List<ILogListener>();
#if !NET_35
        private bool logFirstChanceExceptions = false;
#endif
        private bool logUnhandledExceptions = false;

        /// <summary>
        /// The maximum log level that should be logged
        /// </summary>
        public LogLevel MaxLogLevel { get; set; } = LogLevel.Debug;

        /// <summary>
        /// The default log level
        /// </summary>
        public LogLevel DefaultLogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// The culture used for determining the date time format. The format can always be altered manually afterwards.
        /// </summary>
        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// The format used to format the date & time part of a log message.
        /// </summary>
        public String DateTimeFormat { get; set; } = METRIC_DATETIME_FORMAT;

#if !NET_35        
        /// <summary>
        /// Determines if every thrown exception should be logged, even if it got handled
        /// </summary>
        public bool LogFirstChanceExceptions
        {
            get
            {
                return logFirstChanceExceptions;
            }
            set
            {
                if(value != logFirstChanceExceptions)
                {
                    if(value)
                    {
                        AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
                    }
                    else
                    {
                        AppDomain.CurrentDomain.FirstChanceException -= CurrentDomain_FirstChanceException;
                    }
                    logFirstChanceExceptions = value;
                }
            }
        }
#endif

        /// <summary>
        /// Determines if unhandled exceptions should be logged
        /// </summary>
        public bool LogUnhandledExceptions
        {
            get
            {
                return logUnhandledExceptions;
            }
            set
            {
                if(value != logUnhandledExceptions)
                {
                    if(value)
                    {
                        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    }
                    else
                    {
                        AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
                    }
                    logUnhandledExceptions = value;
                }
            }
        }


        /// <summary>
        /// Logs messages with the given log level.
        /// </summary>
        /// <param name="logLevel">The log level to use</param>
        /// <param name="messages">The messages</param>
        public void Log(LogLevel logLevel, params string[] messages)
        {
            if(logLevel <= MaxLogLevel)
            {
                LogMessage lm = new LogMessage();
                lm.DateTimeFormat = DateTimeFormat;
                lm.Tags.Add(logLevel.ToString());
                foreach(String message in messages)
                {
                    lm.DateTime = DateTime.Now;
                    lm.Message = message;
                    foreach(var l in listeners)
                    {
                        l.Log(lm.ToString());
                    }
                }
            }
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

        public void AddListener(ILogListener listener)
        {
            listener.Attach(this);
            listeners.Add(listener);
        }

        public void RemoveListener(ILogListener listener)
        {
            listener.Detach(this);
            listeners.Remove(listener);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogError("Unhandled Exception: " + e.ExceptionObject.ToString());
            if(e.IsTerminating)
            {
                LogInfo("Terminating...");
            }
        }

#if !NET_35
        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            LogWarning("First Chance Exception: " + e.Exception);
        }
#endif
    }
}
