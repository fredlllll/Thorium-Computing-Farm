using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codolith.Logging.Listeners
{
    public class ConsoleLogListener : ILogListener
    {
        public void Log(string logmessage)
        {
            Console.WriteLine(logmessage);
        }

        public void Attach(Logger logger)
        {
        }

        public void Detach(Logger logger)
        {
        }
    }
}
