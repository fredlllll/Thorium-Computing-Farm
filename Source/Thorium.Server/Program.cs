using NLog;
using Thorium.Plugins;

namespace Thorium.Server
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static ThoriumServer server;
        static void Main(string[] args)
        {
            Logging.Logging.SetupLogging();

            logger.Info("Thorium Server");

            PluginLoader.LoadPlugins();

            server = new ThoriumServer();
            server.Start();
        }
    }
}
