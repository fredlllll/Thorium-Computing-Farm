using System.Net;

namespace Thorium.Server
{
    public class Client
    {
        public IPAddress IPAddress { get; protected set; }
        public string ID { get; protected set; }

        public Client(IPAddress address,string id)
        {
            IPAddress = address;
            ID = id;
        }

        public void Ping()
        {
            //TODO: check if still running, if not, throw exception
        }
    }
}
