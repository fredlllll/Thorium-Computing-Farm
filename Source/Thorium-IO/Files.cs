using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Thorium_Processes;

namespace Thorium_IO
{
    public static class Files
    {
        public static string GetDefault(string file)
        {
            return file + ".default";
        }

        public static List<string> Dirs { get; } = new List<string>();

        static Files()
        {
            Dirs.Add(Directories.ProgramDir);
            Dirs.Add(Directories.AssemblyDir);

            string configFile = Path.Combine(Directories.ProgramDir, "file_dirs.json");
            if(File.Exists(configFile))
            {
                JArray ja = JArray.Parse(File.ReadAllText(configFile));
                foreach(var t in ja)
                {
                    if(t is JObject j && j.Get("load", false))
                    {
                        Dirs.Add(j.Get<string>("path"));
                    }
                }
            }
        }

        static IEnumerable<string> GetFiles(string file)
        {
            IEnumerable<string> retval = Enumerable.Empty<string>();
            foreach(var d in Dirs)
            {
                retval = retval.Concat(System.IO.Directory.EnumerateFiles(d, file, SearchOption.AllDirectories));
            }
            return retval;
        }

        public static string ResolveFileOrDefault(string file)
        {
            //first look for file in all dirs, then for its default
            var retval = GetFiles(file).FirstOrDefault();
            if(retval != null)
            {
                return retval;
            }
            string defaultFile = GetDefault(file);
            retval = GetFiles(defaultFile).FirstOrDefault();
            if(retval != null)
            {
                return retval;
            }
            return null;
        }

        private static Dictionary<string, string> executablesCache = new Dictionary<string, string>();

        /// <summary>
        /// this is needed on unix to find files of executables that you could call in the terminal
        /// </summary>
        /// <param name="executableName"></param>
        /// <returns></returns>
        public static string GetExecutablePath(string executableName)
        {
            if(!executablesCache.TryGetValue(executableName, out string path))
            {
                Process p = ProcessUtil.BeginRunExecutableWithRedirect("/usr/bin/which", executableName);
                path = p.StandardOutput.ReadToEnd().TrimEnd('\r', '\n');
                p.WaitForExit();
            }
            return path;
        }
    }
}
