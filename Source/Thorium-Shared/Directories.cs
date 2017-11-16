using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public static class Directories
    {
        public static string ProgramDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        public static string TempDir
        {
            get
            {
                return Path.GetTempPath();
            }
        }
    }
}
