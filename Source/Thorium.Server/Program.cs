using Thorium.Plugins;

namespace Thorium.Server
{
    class Program
    {
        static ThoriumServer server;
        static void Main(string[] args)
        {
            Logging.Logging.SetupLogging();

            PluginLoader.LoadPlugins();

            server = new ThoriumServer();
            server.Start();
        }
    }
}
