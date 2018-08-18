using System;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq;
using Thorium.CommandLine;
using Thorium.Config;
using Thorium.Net.ServiceHost.Invokers;
using Thorium.Plugins;
using Thorium.Shared;
using Thorium.Shared.Net;

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
