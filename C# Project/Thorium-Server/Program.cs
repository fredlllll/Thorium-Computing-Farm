using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    class Program
    {
        static ThoriumServer server;
        static ConsoleMenu menu;
        static void Main(string[] args)
        {
            server = new ThoriumServer();
            server.Start();

            menu = new ConsoleMenu();
            menu.AddMethod("stop", Stop);
            menu.AddMethod("listtasks", ListTasks);
            menu.AddMethod("help", Help);
            menu.Run();
            Help(null);
        }

        static void Stop(string[] args)
        {
            //lets exit the show
            server.Stop();
            menu.Stop();
        }

        static void ListTasks(string[] args)
        {
            Console.WriteLine("Waiting:");
            foreach(var t in server.TaskManager.Tasks) {
                Console.WriteLine(t.GetID());
            }
            Console.WriteLine("Processing:");
            foreach(var t in server.TaskManager.ProcessingTasks)
            {
                Console.WriteLine(t.GetID()+ " on "+t.GetProcessingClientID());
            }
            Console.WriteLine("Finished:");
            foreach(var t in server.TaskManager.FinishedTasks)
            {
                Console.WriteLine(t.GetID());
            }
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
