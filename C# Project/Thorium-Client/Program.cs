using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thorium_Shared;
using Thorium_Shared.Logging;

namespace Thorium_Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logging.SetupLogging();

            var client = new ThoriumClient();
            client.Start();

            if(args.Contains("-menu"))
            {
                ConsoleMenu menu = new ConsoleMenu();
                //TODO?
                menu.Run();
            }
        }
    }
}
