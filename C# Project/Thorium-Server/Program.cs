using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.Logging;

namespace Thorium_Server
{
    class Program
    {
        static ThoriumServer server;
        static ConsoleMenu menu;
        static void Main(string[] args)
        {
            Logging.SetupLogging();

            server = new ThoriumServer();
            server.Start();

            if(args.Contains("-menu"))
            {
                menu = new ConsoleMenu();
                menu.AddMethod("stop", Stop);
                menu.AddMethod("listjobs", ListJobs);
                menu.AddMethod("help", Help);
                Help(null);
                menu.Run();
            }
        }

        static void Stop(string[] args)
        {
            server.Stop();
            menu.Stop();
        }

        static void ListJobs(string[] args)
        {
            Console.WriteLine("Jobs:");
            /*foreach(var j in server.JobManager.Jobs) {
                Console.WriteLine(j.Value.ID);
            }*/
        }

        static void Help(string[] args)
        {
            Console.WriteLine("available commands:");
            Console.WriteLine("Stop");
            Console.WriteLine("ListTasks");
            Console.WriteLine("Help");
        }
    }
}
