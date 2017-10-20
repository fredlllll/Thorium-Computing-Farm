using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net
{
    public class JSONObjectServer : RestartableThreadClass
    {
        int port;
        TcpListener listener;
        object clientsLock = new object();
        List<JSONObjectClient> clients = new List<JSONObjectClient>();

        object receiverLock = new object();
        public event Action<JObject> ObjectReceived;

        public JSONObjectServer(int port) : base(false)
        {
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
            lock(clientsLock)
            {
                foreach(var c in clients)
                {
                    c.Stop();
                    c.ObjectReceived -= Jc_ObjectReceived;
                    c.Closed -= Jc_Closed;
                }
                clients.Clear();
            }
        }

        protected override void Run()
        {
            try
            {
                while(true)
                {
                    var client = listener.AcceptTcpClient();

                    var jc = new JSONObjectClient(client);
                    jc.Start();
                    jc.ObjectReceived += Jc_ObjectReceived;
                    jc.Closed += Jc_Closed;
                    lock(clientsLock)
                    {
                        clients.Add(jc);
                    }
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

        private void Jc_Closed(JSONObjectClient obj)
        {
            obj.ObjectReceived -= Jc_ObjectReceived;
            obj.Closed -= Jc_Closed;
            lock(clientsLock)
            {
                clients.Remove(obj);
            }
        }

        private void Jc_ObjectReceived(JObject obj)
        {
            lock(receiverLock) //to make sure the event isnt called from 2 threads at once and potentially screw up non thread safe stuff
            {
                ObjectReceived?.Invoke(obj);
            }
        }

        public void Send(JObject obj)
        {
            lock(clientsLock)
            {
                foreach(var c in clients)
                {
                    c.Send(obj);
                }
            }
        }
    }
}
