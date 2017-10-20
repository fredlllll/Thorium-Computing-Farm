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

        public static string GetIfExistsOrDefault(string file)
        {
            if(File.Exists(file))
            {
                return file;
            }
            return GetDefault(file);
        }

        private static Dictionary<string, string> executablePaths = new Dictionary<string, string>();

        public static string GetExecutablePath(string executableName)
        {
            string path = "";
            if(!executablePaths.TryGetValue(executableName, out path))
            {
                Process p = ProcessUtil.BeginRunExecutableWithRedirect("/usr/bin/which", executableName);
                path = p.StandardOutput.ReadToEnd().TrimEnd('\r', '\n');
                p.WaitForExit();
            }
            return path;
        }
    }
}
