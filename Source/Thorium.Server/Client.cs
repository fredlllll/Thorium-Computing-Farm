using System.Net;
using Thorium.Shared;

namespace Thorium.Server
{
    public enum ClientStatus
    {
        Idle,
        Busy
    }

    public class Client
    {
        public string Id { get; protected set; }
        public IPAddress IPAddress { get; protected set; }
        public ClientStatus Status { get; protected set; }

        public Client(string id, IPAddress address, ClientStatus status)
        {
            Id = id;
            IPAddress = address;
            Status = status;
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
