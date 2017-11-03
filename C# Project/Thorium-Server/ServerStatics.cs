using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Codolith.Config;

namespace Thorium_Server
{
    /*public static class ServerStatics
    {
        public static string ServerConfigFolder
        {
            get;
            private set;
        }

        public static void SetServerConfigFolder(string folder)
        {
            folder = Path.GetFullPath(folder);
            if(!Directory.Exists(folder))
            {
                throw new ArgumentException("the folder " + folder + " has to exist");
            }
            ServerConfigFolder = folder;
        }

        public static string ServerConfigPath
        {
            get
            {
                return Path.Combine(ServerConfigFolder, "thorium_server_config.xml");
            }
        }

        private static Config serverConfig = default(Config);
        public static Config ServerConfig
        {
            get
            {
                if(serverConfig == default(Config))
                {
                    serverConfig = new Config(ServerConfigPath);
                }
                return serverConfig;
            }
        }

        static ServerStatics()
        {
            ServerConfigFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
    }*/
}
