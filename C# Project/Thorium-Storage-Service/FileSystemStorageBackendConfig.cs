using System.IO;
using Newtonsoft.Json.Linq;

namespace Thorium_Storage_Service
{
    public static class FileSystemStorageBackendConfig
    {
        public static string StorageDirectory { get; private set; }

        static FileSystemStorageBackendConfig()
        {
            Load();
        }

        public static void Load()
        {
            JObject obj = JObject.Parse(File.ReadAllText(Thorium_Shared.Files.ResolveFileOrDefault(Files.FileSystemStorageBackendConfigFile)));

            StorageDirectory = obj.Get<string>("storageDirectory");
        }
    }
}
