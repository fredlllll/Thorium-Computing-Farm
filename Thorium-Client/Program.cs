using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleMenu menu = new ConsoleMenu();

            var client = new ThoriumClient();

            menu.Run();
        }
    }
}
