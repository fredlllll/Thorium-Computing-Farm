using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Thorium_Logging
{
    public static class Logging
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static LoggingConfiguration LogConfiguration { get; } = new LoggingConfiguration();
        private static Dictionary<LogLevel, LoggingRule> loggingRules = new Dictionary<LogLevel, LoggingRule>();

        public static void SetupLogging()
        {
            AddConsole();

            logger.Info("Logging setup done");
        }

        public static void AddTarget(Target target)
        {
            AddTarget(target, LogLevel.Debug);
        }

        public static void AddTarget(Target target, LogLevel logLevel)
        {
            if(!loggingRules.TryGetValue(logLevel, out LoggingRule lr))
            {
                lr = new LoggingRule();
                foreach(var ll in LogLevel.AllLoggingLevels)
                {
                    if(ll >= logLevel)
                    {
                        lr.EnableLoggingForLevel(ll);
                    }
                    else
                    {
                        lr.DisableLoggingForLevel(ll);
                    }
                }
                lr.LoggerNamePattern = "*";
                LogConfiguration.LoggingRules.Add(lr);
                loggingRules[logLevel] = lr;
            }
            lr.Targets.Add(target);

            LogManager.Configuration = LogConfiguration;
        }

        public static void RemoveTarget(Target t)
        {
            foreach(var kv in loggingRules)
            {
                kv.Value.Targets.Remove(t);
            }
        }

        public static void AddConsole()
        {
            AddConsole(LogLevel.Debug);
        }

        public static void AddConsole(LogLevel logLevel)
        {
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = @"[${date}][${logger}]: ${message}"
            };

            AddTarget(consoleTarget);
        }

        public static void AddLogFile(string file)
        {
            AddLogFile(file, LogLevel.Debug);
        }

        public static void AddLogFile(string file, LogLevel logLevel)
        {
            var fileTarget = new FileTarget
            {
                FileName = file,
                Layout = @"[${date}][${logger}]: ${message}"
            };

            AddTarget(fileTarget);
        }
    }
}
