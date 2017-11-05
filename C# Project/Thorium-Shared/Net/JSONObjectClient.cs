using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net
{
    /*public class JSONObjectClient : RestartableThreadClass, IDisposable
    {
        TcpClient client;
        public TcpClient TcpClient { get { return client; } }
        string host;
        int port;

        public event Action<JSONObjectClient,JObject> ObjectReceived;
        public event Action<JSONObjectClient> Closed;

        public JSONObjectClient(string host, int port) : this(new TcpClient())
        {
            this.host = host;
            this.port = port;
        }

        public JSONObjectClient(TcpClient client) : base(false)
        {
            this.client = client;
        }

        ~JSONObjectClient()
        {
            Dispose(false);
        }

        public void Connect()
        {
            if(!client.Connected)
            {
                client.Connect(host, port);
            }
        }

        public void Disconnect()
        {
            if(client.Connected)
            {
                client.Close();
            }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
            Disconnect();
        }

        void ConnectWithRetry()
        {
            while(!client.Connected)
            {
                try
                {
                    client.Connect(host, port);
                }
                catch(SocketException)
                {
                    //continue
                }
            }
        }

        protected override void Run()
        {
            try
            {
                ConnectWithRetry();

                var sr = new StreamReader(client.GetStream());
                StringBuilder builder = new StringBuilder();
                int indentationLevel = 0;
                while(!sr.EndOfStream)
                {
                    char[] buffer = new char[500];
                    int charsRead = sr.Read(buffer, 0, buffer.Length);
                    for(int i = 0; i < charsRead; i++)
                    {
                        char c = buffer[i];
                        builder.Append(c);
                        if(c == '{')
                        {
                            indentationLevel++;
                        }
                        else if(c == '}')
                        {
                            indentationLevel--;
                            if(indentationLevel == 0)
                            {
                                var obj = JObject.Parse(builder.ToString());
                                ObjectReceived?.Invoke(this, obj);
                                builder.Clear();
                            }
                            else if(indentationLevel < 0)
                            { //error, received partial object, ignore
                                indentationLevel = 0;
                                builder.Clear();
                            }
                        }
                    }
                }
            }
            catch(ThreadInterruptedException)
            {
                //exit
            }
            Closed?.Invoke(this);
        }

        public void Send(JObject obj)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(obj.ToString());
            client.GetStream().Write(bytes, 0, bytes.Length);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                client.Close();
            }
        }
    }*/
}
