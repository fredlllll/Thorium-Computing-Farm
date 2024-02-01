using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Thorium.Shared.Messages;

namespace Thorium.Shared
{
    public class FunctionClient : IDisposable
    {
        private int idCounter = 0;
        private TcpClient client;
        private NetworkStream stream;
        private Thread runThread;

        private readonly Dictionary<int, AutoResetEvent> answerEvents = new();
        private readonly Dictionary<int, FunctionCallAnswer> answers = new();
        private readonly byte[] handshake;

        public FunctionClient(TcpClient client, byte[] handshake)
        {
            this.client = client;
            this.stream = client.GetStream();
            this.handshake = handshake;
        }

        private int GetNextId()
        {
            lock (client)
            {
                idCounter++;
                return idCounter;
            }
        }

        private bool HandshakeSuccessful()
        {
            stream.Write(handshake, 0, handshake.Length);
            byte[] buffer = new byte[handshake.Length];
            stream.ReadExactly(buffer, 0, buffer.Length); //TODO: timeout
            return buffer.SequenceEqual(handshake);
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

        private void HandleMessage(string msg)
        {
            FunctionCallAnswer callAnswer = JsonSerializer.Deserialize<FunctionCallAnswer>(msg, JsonUtil.CaseInsensitive);
            if (callAnswer != null)
            {
                answers[callAnswer.Id] = callAnswer;
                answerEvents[callAnswer.Id].Set();
            }
        }

        private void Run()
        {
            List<byte> buffer = new();
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
            }
        }

        public T RemoteFunctionCall<T>(string functionName, bool needsAnswer, params object[] args)
        {
            int id = GetNextId();

            var call = new FunctionCall
            {
                Id = id,
                FunctionName = functionName,
                NeedsAnwer = needsAnswer,
                FunctionArguments = args
            };

            byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(call));

            using var answerEvent = answerEvents[id] = new AutoResetEvent(false);
            stream.Write(bytes, 0, bytes.Length);
            stream.WriteByte(0);

            if (needsAnswer)
            {
                answerEvent.WaitOne(); //wait for answer to arrive
                answerEvents.Remove(id);
                var answer = answers[id];
                answers.Remove(id);
                if (answer.Exception != null)
                {
                    throw answer.Exception; //TODO no idea if this works lel
                }
                return (T)answer.ReturnValue;
            }
            return default;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
