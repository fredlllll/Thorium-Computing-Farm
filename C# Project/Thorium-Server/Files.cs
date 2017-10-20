using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public static class Files
    {
        public static string ThoriumServerConfigFile
        {
            get { return Path.Combine(Directories.ProgramDir, "thorium_server_config.json"); }
        }
    }
}
