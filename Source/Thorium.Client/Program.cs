using System.Linq;
using NLog;
using Thorium.Plugins;

namespace Thorium.Client
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logging.Logging.SetupLogging();

            logger.Info("Thorium Client");

            PluginLoader.LoadPlugins();

            var client = new ThoriumClient();
            client.Start();
        }
    }
}
