using System;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace Thorium_Net
{
    public class TCPServiceInvoker : IServiceInvoker, IDisposable
    {
        TcpClient client;

        BinaryWriter writer;
        BinaryReader reader;

        public TCPServiceInvoker(TcpClient client)
        {
            this.client = client;
            writer = new BinaryWriter(client.GetStream());
            reader = new BinaryReader(client.GetStream());
        }

        public TCPServiceInvoker(string host, int port) : this(new TcpClient(host, port))
        {

        }

        public JToken Invoke(string routine, JToken arg)
        {
            writer.Write(routine);
            writer.Write(arg.ToString(Newtonsoft.Json.Formatting.None));

            Byte state = reader.ReadByte();
            switch(state)
            {
                case 0: //exception
                    throw new Exception(reader.ReadString());
                case 1: //success with return
                    return JToken.Parse(reader.ReadString());
                case 2: //success
                    return null;
                default:
                    throw new Exception("parsing error when receiving response. you might want to reconnect after this");
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    reader.Dispose();
                    writer.Dispose();
                    client.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
