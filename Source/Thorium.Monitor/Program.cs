using System;
using System.Windows.Forms;
using Thorium_Logging;
using Thorium_Plugins;

namespace Thorium_Monitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logging.SetupLogging();

            PluginLoader.LoadPlugins();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
