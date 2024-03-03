using NLog;
using System.IO;
using System.Reflection;
using Thorium.Shared;

namespace Thorium.Server
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static ThoriumServer server;
        static void Main(string[] args)
        {
            Logging.SetupLogging();
            logger.Info("Thorium Server v" + Assembly.GetEntryAssembly()?.GetName().Version);

            Settings.LoadJson("settings.json");
            if (File.Exists("local_settings.json"))
            {
                Settings.LoadJson("local_settings.json");
            }
            
            server = new ThoriumServer();
            server.Run();
        }
    }
}
