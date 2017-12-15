/*using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.Comms
{
    public delegate void MessageReceivedHandler(IMessageTransceiver sender, JObject msg);
    public delegate void TransceiverClosedHandler(IMessageTransceiver sender);

    public interface IMessageTransceiver : IDisposable
    {
        event MessageReceivedHandler MessageReceived;
        event TransceiverClosedHandler Closed;

        IPAddress Remote
        {
            get;
        }

        void SendMessage(JObject msg);

        void Start();
        void Stop();
    }
}
*/