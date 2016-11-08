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
        }

        static void Stop(string[] args)
        {
            //lets exit the show
            server.Stop();
            menu.Stop();
        }
    }
}
