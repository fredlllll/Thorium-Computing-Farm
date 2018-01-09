using System.IO;
using System.Reflection;

namespace Thorium_Shared
{
    public static class Directories
    {
        /// <summary>
        /// Gives the directory that contains the assembly that called this
        /// </summary>
        public static string AssemblyDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            }
        }

        /// <summary>
        /// Gives the directory that contains the entry assembly
        /// </summary>
        public static string ProgramDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        /// <summary>
        /// Gives the users (windows) or systems (linux) temp directory
        /// </summary>
        public static string TempDir
        {
            get
            {
                return Path.GetTempPath();
            }
        }
    }
}
