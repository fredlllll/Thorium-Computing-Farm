using System.Linq;
using Thorium.CommandLine;
using Thorium.Plugins;

namespace Thorium.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logging.Logging.SetupLogging();

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
