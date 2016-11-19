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
                Console.WriteLine(t.ID);
            }
            Console.WriteLine("Processing:");
            foreach(var t in server.TaskManager.ProcessingTasks)
            {
                Console.WriteLine(t.ID+ " on "+t.ProcessingClientID);
            }
            Console.WriteLine("Finished:");
            foreach(var t in server.TaskManager.FinishedTasks)
            {
                Console.WriteLine(t.ID);
            }
        }

        static void Help(string[] args)
        {
            Console.WriteLine("available commands:");
            Console.WriteLine("Stop");
            Console.WriteLine("ListTasks");
        }
    }
}
