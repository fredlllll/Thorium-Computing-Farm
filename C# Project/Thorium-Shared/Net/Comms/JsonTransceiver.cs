using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.Comms
{
    public class JsonTransceiver : RestartableThreadClass, IMessageTransceiver
    {
        private readonly TcpClient client;
        private JsonTextWriter jsonWriter = null;

        public IPAddress Remote => ((IPEndPoint)client.Client.RemoteEndPoint).Address;

        public event MessageReceivedHandler MessageReceived;
        public event TransceiverClosedHandler Closed;

        public JsonTransceiver(TcpClient client) : base(false)
        {
            this.client = client;
        }

        public override void Start()
        {
            StreamWriter sw = new StreamWriter(client.GetStream());
            jsonWriter = new JsonTextWriter(sw);
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            jsonWriter.CloseOutput = true;
            jsonWriter.Close();
        }

        public void SendMessage(JObject msg)
        {
            msg.WriteTo(jsonWriter);
            jsonWriter.Flush();
        }

        protected override void Run()
        {
            try
            {
                using(StreamReader sr = new StreamReader(client.GetStream()))
                using(JsonTextReader jr = new JsonTextReader(sr))
                {
                    while(!sr.EndOfStream)
                    {
                        JObject jobj = JObject.Load(jr);
                        MessageReceived?.Invoke(this, jobj);
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                //exit
            }
            Closed?.Invoke(this);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    Stop();
                    MessageReceived = null;
                    Closed = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~JsonTransceiver() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
