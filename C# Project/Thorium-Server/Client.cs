using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Server
{
    public class Client
    {
        public IPAddress IPAddress { get; protected set; }

        public string CurrentTask { get; set; }

        public Client(IPAddress address)
        {
            IPAddress = address;
        }

        public void Ping()
        {
            //TODO: check if still running, if not, throw exception
        }
    }
}
