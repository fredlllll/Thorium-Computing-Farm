using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public static class Files
    {
        public static string GetDefault(string file)
        {
            return file + ".default";
        }

        public static string ResolveFileOrDefault(string file)
        {
            var files = Directory.EnumerateFiles(Directories.ProgramDir, file, SearchOption.AllDirectories);
            var retval = files.FirstOrDefault();
            if(retval != null)
            {
                return retval;
            }
            string defaultFile = GetDefault(file);
            files = Directory.EnumerateFiles(Directories.ProgramDir, defaultFile, SearchOption.AllDirectories);
            retval = files.FirstOrDefault();
            if(retval != null)
            {
                return retval;
            }
            return file;
        }

        private static Dictionary<string, string> executablePaths = new Dictionary<string, string>();

        /// <summary>
        /// this is needed on unix to find files of executables that you could call in the terminal
        /// </summary>
        /// <param name="executableName"></param>
        /// <returns></returns>
        public static string GetExecutablePath(string executableName)
        {
            if(!executablePaths.TryGetValue(executableName, out string path))
            {
                Process p = ProcessUtil.BeginRunExecutableWithRedirect("/usr/bin/which", executableName);
                path = p.StandardOutput.ReadToEnd().TrimEnd('\r', '\n');
                p.WaitForExit();
            }
            return path;
        }
    }
}
