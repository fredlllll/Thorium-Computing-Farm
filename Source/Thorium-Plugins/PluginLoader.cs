using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using NLog;
using Thorium_IO;

namespace Thorium_Plugins
{
    public static class PluginLoader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void LoadPlugins()
        {
            logger.Info("Loading plugins");
            var pluginsFile = Path.Combine(Directories.ProgramDir, "plugins.json");
            if(File.Exists(pluginsFile))
            {
                JArray plugins = JArray.Parse(File.ReadAllText(pluginsFile));
                foreach(JObject jo in plugins)
                {
                    bool load = jo.Get<bool>("load");
                    if(load)
                    {
                        string name = jo.Get<string>("name", "unnamed");
                        string file = jo.Get<string>("file");
                        if(!File.Exists(file))
                        {
                            logger.Warn("file doesnt exist: " + file);
                            continue;
                        }

                        var assemblyName = AssemblyName.GetAssemblyName(file);
                        AppDomain.CurrentDomain.Load(assemblyName);
                        logger.Info("Loaded assembly " + name + ": " + file);
                    }
                }
            }
            else
            {
                logger.Warn("No plugins.json found. cant load plugins");
            }

            /*var files = Directory.EnumerateFiles(Directories.PluginsDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            foreach(var file in files)
            {
                var assemblyName = AssemblyName.GetAssemblyName(file);
                AppDomain.CurrentDomain.Load(assemblyName);
                logger.Info("Loaded assembly: " + file);
            }*/
        }
    }
}
