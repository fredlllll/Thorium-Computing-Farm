using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared.Aether;
using Thorium.Shared.Aether.AetherSerializers;
using Thorium.Shared.DTOs;
using Thorium.Shared.Messages;

namespace Thorium.Shared
{
    public class FunctionServer
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly TcpClient client;
        private readonly NetworkStream stream;
        private readonly Dictionary<string, Tuple<MethodInfo, object>> functions = [];
        private readonly byte[] handshake;
        private readonly AetherStream aether;

        private Thread runThread;

        public FunctionServer(TcpClient client, byte[] handshake)
        {
            this.client = client;
            stream = client.GetStream();
            this.handshake = handshake;
            aether = new AetherStream(stream);
            aether.Serializers[typeof(FunctionCall)] = new FunctionCallSerializer();
            aether.Serializers[typeof(FunctionCallAnswer)] = new FunctionCallAnswerSerializer();
        }

        public void AddFunction(MethodInfo methodInfo, object target)
        {
            functions[methodInfo.Name] = new Tuple<MethodInfo, object>(methodInfo, target);
        }

        private bool HandshakeSuccessful()
        {
            byte[] buffer = new byte[handshake.Length];
            //TODO: add timeout
            stream.ReadExactly(buffer, 0, buffer.Length);

            bool clientHandshake = buffer.SequenceEqual(handshake);
            if (clientHandshake)
            {
                stream.Write(handshake, 0, handshake.Length);
                return true;
            }
            return false;
        }

        public void Start()
        {
            if (!HandshakeSuccessful())
            {
                client.Close();
                throw new Exception("Handshake unsuccessful");
            }
            else
            {
                runThread = new Thread(Run);
                runThread.Start();
            }
        }

        private void SendAnswer(int id, object result, Exception exception)
        {
            var answer = new FunctionCallAnswer();
            answer.Id = id;
            answer.ReturnValue = result;
            answer.Exception = exception?.ToString();

            /*var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(answer));
            stream.Write(bytes, 0, bytes.Length);
            stream.WriteByte(0);*/
            aether.Write(answer);
        }

        private void HandleMessage(string msg)
        {
            FunctionCall call = JsonSerializer.Deserialize<FunctionCall>(msg, JsonUtil.CaseInsensitive);
            if (call != null)
            {
                if (functions.TryGetValue(call.FunctionName, out var func))
                {
                    object result = null;
                    Exception exception = null;
                    try
                    {
                        result = func.Item1.Invoke(func.Item2, call.FunctionArguments);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    SendAnswer(call.Id, result, exception);
                }
            }
        }

        private void Run()
        {
            /*List<byte> buffer = new();
            byte[] readBuffer = new byte[16 * 1024];
            while (true)
            {
                //TODO: timeout? handle stream close
                int count = stream.Read(readBuffer, 0, readBuffer.Length);
                for (int i = 0; i < count; i++)
                {
                    byte b = readBuffer[i];
                    if (b == 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count); //TODO: try to get around creating a new array every time
                        HandleMessage(message);
                        buffer.Clear();
                    }
                    else
                    {
                        buffer.Add(b);
                    }
                }
            }*/
            while (true)
            {
                var call = (FunctionCall)aether.Read();
                if (functions.TryGetValue(call.FunctionName, out var func))
                {
                    object result = null;
                    Exception exception = null;
                    try
                    {
                        result = func.Item1.Invoke(func.Item2, call.FunctionArguments);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    SendAnswer(call.Id, result, exception);
                }
                else
                {
                    logger.Error("got call for unknown function " + call.FunctionName);
                }
            }
        }
    }
}
