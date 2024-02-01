using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Thorium.Shared;

namespace Thorium.Client
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logging.SetupLogging();
            logger.Info("Thorium Client v" + Assembly.GetEntryAssembly()?.GetName().Version);

            Settings.LoadJson("settings.json");
            if (File.Exists("local_settings.json"))
            {
                Settings.LoadJson("local_settings.json");
            }

            //dependency injection
            DI.Services.AddSingleton<ThoriumServerApi>((x) =>
            {
                var api = new ThoriumServerApi();
                api.Start();
                return api;
            });

            var client = new ThoriumClient();
            client.Run();
        }
    }
}
