/*using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using NLog;

namespace Thorium_Shared.Net.Comms
{
    public class JsonTransceiver : RestartableThreadClass, IMessageTransceiver
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly TcpClient client;
        private BinaryWriter binaryWriter = null;

        public IPAddress Remote => ((IPEndPoint)client.Client.RemoteEndPoint).Address;

        public event MessageReceivedHandler MessageReceived;
        public event TransceiverClosedHandler Closed;

        public JsonTransceiver(TcpClient client) : base(false)
        {
            this.client = client;
        }

        public override void Start()
        {
            binaryWriter = new BinaryWriter(client.GetStream());
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            binaryWriter.Close();
        }

        public void SendMessage(JObject msg)
        {
            binaryWriter.Write(msg.ToString());
            binaryWriter.Flush();
        }

        protected override void Run()
        {
            try
            {
                using(BinaryReader br = new BinaryReader(client.GetStream()))
                {
                    try
                    {
                        while(client.Connected)
                        {
                            string jsonString = br.ReadString();
                            JObject jobj = JObject.Parse(jsonString);
                            MessageReceived?.Invoke(this, jobj);
                        }
                    }
                    catch(IOException)
                    {
                        logger.Info("Transceiver stream closed");
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                logger.Info("transceiver interrupted");
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
*/