using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Server
{
    public class ClientTaskRelation
    {
        public string Client { get; set; }
        public string Task { get; set; }

        public ClientTaskRelation(string client, string task)
        {
            Client = client;
            Task = task;
        }
    }
}
