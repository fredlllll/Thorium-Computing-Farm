using System.Linq;
using Thorium_CommandLine;
using Thorium_Logging;
using Thorium_Plugins;

namespace Thorium_Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logging.SetupLogging();

            PluginLoader.LoadPlugins();

            var client = new ThoriumClient();
            client.Start();

            if(args.Contains("-menu"))
            {
                ConsoleMenu menu = new ConsoleMenu();
                //TODO?
                menu.Run();
            }
        }
    }
}
