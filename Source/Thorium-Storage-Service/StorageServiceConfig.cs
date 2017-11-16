using System.IO;
using Newtonsoft.Json.Linq;

namespace Thorium_Storage_Service
{
    public static class StorageServiceConfig
    {
        public static string StorageBackend { get; private set; }

        static StorageServiceConfig()
        {
            Load();
        }

        public static void Load()
        {
            JObject obj = JObject.Parse(File.ReadAllText(Thorium_Shared.Files.ResolveFileOrDefault(Files.ThoriumStorageServiceConfigFile)));

            StorageBackend = obj.Get<string>("storageBackend");
        }
    }
}
