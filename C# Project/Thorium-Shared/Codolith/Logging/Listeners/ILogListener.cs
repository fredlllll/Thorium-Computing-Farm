using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codolith.Logging.Listeners
{
    public interface ILogListener
    {
        void Log(string logmessage);

        void Attach(Logger logger);
        void Detach(Logger logger);
    }
}
