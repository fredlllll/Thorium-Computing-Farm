using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Thorium_Shared.Logging
{
    public static class Logging
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static LoggingConfiguration lc = new LoggingConfiguration();

        public static void SetupLogging()
        {
            AddConsole("console");

            logger.Info("Logging setup done");
        }

        public static void AddConsole(string name)
        {
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = @"[${date}][${logger}]: ${message}"
            };

            lc.AddTarget(name, consoleTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);

            lc.LoggingRules.Add(rule);

            LogManager.Configuration = lc;
        }

        public static void AddLogFile(string name, string file)
        {
            var fileTarget = new FileTarget
            {
                FileName = file,
                Layout = @"[${date}][${logger}]: ${message}"
            };

            lc.AddTarget(name, fileTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);

            lc.LoggingRules.Add(rule);

            LogManager.Configuration = lc;
        }

        /*public static void AddCloudWatch(BasicAWSCredentials credentials, RegionEndpoint region, string groupName, string streamName)
        {
            var cloudWatchTarget = new CloudWatchTarget(credentials, region)
            {
                GroupName = groupName,
                StreamName = streamName,
            };

            lc.AddTarget("cloudwatch", cloudWatchTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, cloudWatchTarget);

            lc.LoggingRules.Add(rule);

            LogManager.Configuration = lc;
        }*/
    }
}
