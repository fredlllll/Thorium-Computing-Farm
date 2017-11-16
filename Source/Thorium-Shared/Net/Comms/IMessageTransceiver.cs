using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
