using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.Comms
{
    public delegate JObject InvokationReceivedHandler(IMessageTransceiver sender, string command, JObject arg);

    public class ServiceServer : RestartableThreadClass
    {
        private readonly ushort port;

        private readonly Type messageTransceiverType;
        private ConstructorInfo messageTransceiverConstructor;

        private TcpListener listener;

        private readonly object transceiversLock = new object();
        private readonly List<IMessageTransceiver> transceivers = new List<IMessageTransceiver>();

        public event InvokationReceivedHandler InvokationReceived;

        public ServiceServer(Type messageTransceiverType, ushort port) : base(false)
        {
            this.messageTransceiverType = messageTransceiverType;
            this.port = port;
        }

        public override void Start()
        {
            if(listener == null)
            {
                listener = new TcpListener(IPAddress.Any, port);
            }
            listener.Start();
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            listener.Stop();
            lock(transceiversLock)
            {
                foreach(var c in transceivers)
                {
                    c.Dispose();
                }
                transceivers.Clear();
            }
        }

        private void Transceiver_MessageReceived(IMessageTransceiver sender, JObject msg)
        {
            long invokID = msg.Get<long>("invokationID");
            String command = msg.Get<string>("command");
            JObject arg = (JObject)msg["arg"];

            JObject response = null;
            if(InvokationReceived != null)
            {
                response = InvokationReceived(sender, command, arg);
            }

            JObject responseMessage = new JObject()
            {
                ["invokationID"] = invokID,
                ["response"] = response
            };

            sender.SendMessage(responseMessage);
        }

        private void Transceiver_Closed(IMessageTransceiver sender)
        {
            sender.Dispose();
            lock(transceiversLock)
            {
                transceivers.Remove(sender);
            }
        }

        private IMessageTransceiver GetMessageTransceiver(TcpClient client)
        {
            if(messageTransceiverConstructor == null)
            {
                messageTransceiverConstructor = messageTransceiverType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(TcpClient) }, null);
                if(messageTransceiverConstructor == null)
                {
                    throw new Exception("the given " + nameof(messageTransceiverType) + " does not have a constructor taking a " + nameof(TcpClient));
                }
            }
            return (IMessageTransceiver)messageTransceiverConstructor.Invoke(new object[] { client });
        }

        protected override void Run()
        {
            try
            {
                while(true)
                {
                    var client = listener.AcceptTcpClient();

                    var transceiver = GetMessageTransceiver(client);
                    transceiver.MessageReceived += Transceiver_MessageReceived;
                    transceiver.Closed += Transceiver_Closed;
                    lock(transceiversLock)
                    {
                        transceivers.Add(transceiver);
                    }
                    transceiver.Start();
                }
            }

            catch(ObjectDisposedException)
            {
                //occurs when listener was disposed due to closing it
            }
            catch(ThreadInterruptedException)
            {
                //ending
            }
        }
    }
}
