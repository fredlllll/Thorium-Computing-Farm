using NLog;
using System.IO;
using System.Reflection;
using Thorium.Shared;

namespace Thorium.Server
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logging.SetupLogging();
            logger.Info("Thorium Server v" + Assembly.GetEntryAssembly()?.GetName().Version);

            if (File.Exists("local_settings.json"))
            {
                Settings.LoadJson("local_settings.json");
            }
            
            var server = new ThoriumServer();
            server.Start();
            server.Run();
        }
    }
}
