using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Storage_Slave
{
    public static class Program
    {
        static ThoriumStorageSlave slave;
        static void Main(string[] args)
        {
            ConsoleMenu menu = new ConsoleMenu();

            slave = new ThoriumStorageSlave();

            menu.AddMethod("shutdown", Program.shutdown);
            menu.Run();
        }

        static void shutdown(string[] args)
        {
            slave.Shutdown();
        }
    }
}
