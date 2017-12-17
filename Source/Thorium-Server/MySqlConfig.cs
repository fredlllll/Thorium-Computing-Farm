using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Thorium_Server
{
    public static class MySqlConfig
    {
        public static string DatabaseHost { get; private set; }
        public static UInt16 DatabasePort { get; private set; }
        public static string DatabaseUser { get; private set; }
        public static string DatabasePassword { get; private set; }
        public static string DatabaseName { get; private set; }
        public static string TablePrefix { get; private set; }

        static MySqlConfig()
        {
            Load();
        }

        public static void Load()
        {
            JObject obj = JObject.Parse(File.ReadAllText(Thorium_Shared.Files.ResolveFileOrDefault(Files.MySqlConfigFile)));

            DatabaseHost = obj.Get<string>("databaseHost");
            DatabasePort = obj.Get<UInt16>("databasePort");
            DatabaseUser = obj.Get<string>("databaseUser");
            DatabasePassword = obj.Get<string>("databasePassword");
            DatabaseName = obj.Get<string>("databaseName");
            TablePrefix = obj.Get<string>("tablePrefix");
        }
    }
}
