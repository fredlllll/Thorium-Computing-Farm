using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Thorium.Shared;
using Thorium.Shared.Util;

namespace Thorium.Client
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logging.SetupLogging();
            logger.Info("Thorium Client v" + Assembly.GetEntryAssembly()?.GetName().Version);

            Init();

            var client = new ThoriumClient();
            logger.Info("Starting");
            client.Run();
        }

        static void InitSettings()
        {
            Settings.LoadJson("settings.json");
            if (File.Exists("local_settings.json"))
            {
                Settings.LoadJson("local_settings.json");
            }
            logger.Info("Loaded Settings");
        }

        static void InitConnections()
        {
            DI.Services.AddSingleton<ThoriumServerApi>();

            var tsa = DI.ServiceProvider.GetRequiredService<ThoriumServerApi>();
            tsa.ServerHost = Settings.Get<string>("serverHost");
            tsa.ServerPort = Settings.Get<int>("serverPort");

            logger.Info("Initialized Connection");
        }

        static void Init()
        {
            InitSettings();
            InitConnections();
        }
    }
}
