using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Client
{
    public static class Files
    {
        public static string ThoriumClientConfigFile
        {
            get { return Path.Combine(Directories.ProgramDir, "thorium_client_config.json"); }
        }
    }
}
