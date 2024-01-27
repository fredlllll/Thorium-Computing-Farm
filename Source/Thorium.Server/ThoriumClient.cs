using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared.DTOs;
using Thorium.Shared.Messages;

namespace Thorium.Server
{
    public class ThoriumClient
    {
        private readonly TcpClient client;
        private readonly ThoriumServer server;
        private readonly NetworkStream stream;

        private Dictionary<string, MethodInfo> functions;
        private Thread runThread;

        public DateTime LastHeartbeat { get; private set; }

        public ThoriumClient(TcpClient client, ThoriumServer server) {
            this.client = client;
            this.server = server;
            this.stream = client.GetStream();
        }

        public bool HandshakeSuccessful()
        {
            byte[] buffer = new byte[4]; 
            //TODO: add timeout
            stream.ReadExactly(buffer, 0, buffer.Length);

            return buffer[0] == 'T' && buffer[0] == 'H' && buffer[0] == 'O' && buffer[0] == 'R';
        }

        public void Start()
        {
            functions = new Dictionary<string, MethodInfo>();

            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            functions[nameof(Heartbeat)] = GetType().GetMethod(nameof(Heartbeat), flags);
            functions[nameof(GetTask)] = GetType().GetMethod(nameof(GetTask), flags);
            functions[nameof(FinishTask)] = GetType().GetMethod(nameof(FinishTask), flags);
            functions[nameof(Log)] = GetType().GetMethod(nameof(Log), flags);

            runThread = new Thread(Run);
            runThread.Start();
        }

        void SendAnswer (int id, object? result, Exception? exception)
        {
            var answer = new FunctionCallAnswer();
            answer.Id = id;
            answer.ReturnValue = result;
            answer.Exception = exception;

            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(answer));
            stream.Write(bytes, 0, bytes.Length);
            stream.WriteByte(0);
        }

        void HandleMessage(string msg)
        {
            FunctionCall call = JsonSerializer.Deserialize<FunctionCall>(msg);
            if(call != null)
            {
                if(functions.TryGetValue(call.FunctionName, out var func))
                {
                    object result = null;
                    Exception exception = null;
                    try
                    {
                         result = func.Invoke(this, call.FunctionArguments);
                    }catch (Exception ex)
                    {
                        exception = ex;
                    }
                    SendAnswer(call.Id,result, exception);
                }
            }
        }

        void Run()
        {
            LastHeartbeat = DateTime.Now;
            List<byte> buffer = new();
            byte[] readBuffer = new byte[16 * 1024];
            while (true)
            {
                //TODO: timeout? handle stream close
                int count = stream.Read(readBuffer, 0, readBuffer.Length);
                for (int i = 0; i < count; i++)
                {
                    byte b = readBuffer[i];
                    if(b == 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count); //TODO: try to get around creating a new array every time
                        HandleMessage(message);
                        buffer.Clear();
                    }
                    buffer.Add(b);
                }
            }
        }

        #region functions
        void Heartbeat()
        {
            LastHeartbeat = DateTime.Now;
        }

        ThoriumTask GetTask()
        {
            return null;//TODO
        }

        void FinishTask(string taskId)
        {
            //TODO
        }

        void Log(string msg)
        {
            //TODO
        }
        #endregion

    }
}
