using System.Net;
using Thorium.Shared;

namespace Thorium.Server
{
    public class Client
    {
        public IPAddress IPAddress { get; protected set; }
        public string Id { get; protected set; }

        public Client(IPAddress address,string id)
        {
            IPAddress = address;
            Id = id;
        }

        public bool Ping()
        {
            //TODO: check if still running, if not, throw exception
            return true;
        }

        public bool AssignTask(LightweightTask task)
        {
            return true;//TODO: rest
        }
    }
}
