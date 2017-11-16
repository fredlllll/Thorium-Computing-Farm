using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Storage_Service
{
    public static class Files
    {
        public static string ThoriumStorageServiceConfigFile
        {
            get
            {
                return "thorium_storage_service_config.json";
            }
        }

        public static string FileSystemStorageBackendConfigFile
        {
            get
            {
                return "file_system_storage_backend_config.json";
            }
        }
    }
}
