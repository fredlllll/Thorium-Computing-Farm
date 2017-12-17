using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Thorium_Server
{
    public static class ThoriumServerConfig
    {
        public static UInt16 ListeningPort { get; private set; }
        public static UInt16 ClientListeningPort { get; private set; }
       

        static ThoriumServerConfig()
        {
            Load();
        }

        public static void Load()
        {
            JObject obj = JObject.Parse(File.ReadAllText(Thorium_Shared.Files.ResolveFileOrDefault(Files.ThoriumServerConfigFile)));

            ListeningPort = obj.Get<UInt16>("listeningPort");
            ClientListeningPort = obj.Get<UInt16>("clientListeningPort");
        }
    }
}
