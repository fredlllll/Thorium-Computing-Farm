using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Thorium_Server
{
    public static class ThoriumServerConfig
    {
        public static UInt16 ListeningPort { get; private set; }
        public static UInt16 ClientListeningPort { get; private set; }
        public static string DatabaseHost { get; private set; }
        public static UInt16 DatabasePort { get; private set; }
        public static string DatabaseUser { get; private set; }
        public static string DatabasePassword { get; private set; }

        static ThoriumServerConfig()
        {
            Load();
        }

        public static void Load()
        {
            JObject obj = JObject.Parse(File.ReadAllText(Thorium_Shared.Files.GetIfExistsOrDefault(Thorium_Server.Files.ThoriumServerConfigFile)));

            ListeningPort = obj.Get<UInt16>("listeningPort");
            ClientListeningPort = obj.Get<UInt16>("clientListeningPort");
            DatabaseHost = obj.Get<string>("databaseHost");
            DatabasePort = obj.Get<UInt16>("databasePort");
            DatabaseUser = obj.Get<string>("databaseUser");
            DatabasePassword = obj.Get<string>("databasePassword");
        }
    }
}
