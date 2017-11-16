using System;
using System.Collections.Concurrent;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.Comms
{
    public class ServiceClient
    {
        IMessageTransceiver transceiver;

        object invokationCounterLock = new object();
        long invokationCounter = 0;

        ConcurrentDictionary<long, ManualResetEvent> responseWaiters = new ConcurrentDictionary<long, ManualResetEvent>();
        ConcurrentDictionary<long, JObject> responses = new ConcurrentDictionary<long, JObject>();

        public ServiceClient(IMessageTransceiver transceiver)
        {
            this.transceiver = transceiver;
            transceiver.MessageReceived += Transceiver_MessageReceived;
            transceiver.Start();
        }

        public JObject Invoke(string command, JObject arg)
        {
            long invokID;
            lock(invokationCounterLock)
            {
                invokID = invokationCounter++;
            }
            JObject msg = new JObject
            {
                ["invokationID"] = invokID,
                ["command"] = command,
            };
            if(arg != null)
            {
                msg["arg"] = arg;
            }
            ManualResetEvent mre = new ManualResetEvent(false);
            responseWaiters[invokID] = mre;

            transceiver.SendMessage(msg);

            if(!mre.WaitOne(10000))
            {
                throw new TimeoutException("Timeout occured when calling " + command);
            }
            responseWaiters.TryRemove(invokID, out mre);

            responses.TryRemove(invokID, out JObject response);
            return response;
        }

        private void Transceiver_MessageReceived(IMessageTransceiver sender, JObject msg)
        {
            long invokID = msg.Get<long>("invokationID");
            var resp = msg["response"];
            if(resp != null && !resp.IsNull())
            {
                JObject response = (JObject)resp;
                responses[invokID] = response;
            }
            else
            {
                responses[invokID] = null;
            }
            responseWaiters[invokID].Set();
        }
    }
}
