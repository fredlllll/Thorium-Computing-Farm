using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using NLog;

namespace Thorium_Shared.Net.ServicePoint
{
    public class TCPServiceInvokationReceiver : IServiceInvokationReceiver
    {
        public event InvokationHandler InvokationReceived;

        ConcurrentList<Receiver> clients = new ConcurrentList<Receiver>();

        TcpListener listener;

        public TCPServiceInvokationReceiver(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            listener.BeginAcceptTcpClient(AcceptClient, null);
        }

        public void Stop()
        {
            listener.Stop();

            foreach(var client in clients)
            {
                client.Dispose();
            }
            clients.Clear();
        }

        void AcceptClient(IAsyncResult res)
        {
            var client = listener.EndAcceptTcpClient(res);

            var receiver = new Receiver(this, client);
            receiver.Start();

            clients.Add(receiver);
        }

        InvokationResult RaiseInvokation(string routine, JToken arg)
        {
            return InvokationReceived?.Invoke(this, routine, arg);
        }

        void RemoveReceiver(Receiver receiver)
        {
            clients.Remove(receiver);
        }

        class Receiver : RestartableThreadClass, IDisposable
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            private readonly TCPServiceInvokationReceiver receiver;
            private readonly TcpClient client;
            private BinaryReader reader;
            private BinaryWriter writer;

            public Receiver(TCPServiceInvokationReceiver receiver, TcpClient client) : base(false)
            {
                this.receiver = receiver;
                this.client = client;

                reader = new BinaryReader(client.GetStream());
                writer = new BinaryWriter(client.GetStream());
            }

            protected override void Run()
            {
                try
                {
                    while(client.Connected)
                    {
                        try
                        {
                            string routine = reader.ReadString();
                            string argString = reader.ReadString();

                            JToken arg = null;
                            if(argString.Length > 0)
                            {
                                arg = JToken.Parse(argString);
                            }

                            InvokationResult result = receiver.RaiseInvokation(routine, arg);
                            if(result.Exception != null) //exception
                            {
                                writer.Write((byte)0);
                                writer.Write(result.Exception.ToString());
                            }
                            else if(result.ReturnValue != null) //success with returnvalue
                            {
                                writer.Write((byte)1);
                                writer.Write(result.ReturnValue.ToString());
                            }
                            else //success without returnvalue or null
                            {
                                writer.Write((byte)2);
                            }
                        }
                        catch(Exception ex) when(!(ex is ThreadInterruptedException))
                        {
                            logger.Warn(nameof(TCPServiceInvokationReceiver) + " threw an exception when receiving: " + ex.ToString());
                        }
                    }
                }
                catch(ThreadInterruptedException)
                {
                    //end
                }
                receiver.RemoveReceiver(this);
                Dispose();
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if(!disposedValue)
                {
                    disposedValue = true;
                    if(disposing)
                    {
                        Stop();

                        client.Dispose();
                        reader.Dispose();
                        writer.Dispose();
                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }
            #endregion
        }
    }
}
