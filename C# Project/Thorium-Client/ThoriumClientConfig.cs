using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Thorium_Client
{
    public static class ThoriumClientConfig
    {
        public static UInt16 ServerListeningPort { get; private set; }
        public static string ServerHost { get; private set; }

        static ThoriumClientConfig()
        {
            Load();
        }

        public static void Load()
        {
            JObject obj = JObject.Parse(File.ReadAllText(Thorium_Shared.Files.GetIfExistsOrDefault(Thorium_Client.Files.ThoriumClientConfigFile)));

            ServerListeningPort = obj.Get<UInt16>("serverListeningPort");
            ServerHost = obj.Get<string>("serverHost");
        }
    }
}
