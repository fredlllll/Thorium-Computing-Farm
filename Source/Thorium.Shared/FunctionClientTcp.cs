using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Thorium.Shared.Aether;
using Thorium.Shared.Aether.AetherSerializers;
using Thorium.Shared.Messages;

namespace Thorium.Shared
{
    public class FunctionClientTcp : IDisposable
    {
        private int callIdCounter = 0;
        private readonly TcpClient client;
        private readonly NetworkStream stream;
        private readonly Dictionary<int, AutoResetEvent> answerEvents = [];
        private readonly Dictionary<int, FunctionCallAnswer> answers = [];
        private readonly byte[] handshake;

        private Thread runThread;

        public AetherStream Aether { get; }

        public FunctionClientTcp(TcpClient client, byte[] handshake)
        {
            this.client = client;
            stream = client.GetStream();
            this.handshake = handshake;
            Aether = new AetherStream(stream);
            Aether.Serializers[typeof(FunctionCall)] = new FunctionCallSerializer();
            Aether.Serializers[typeof(FunctionCallAnswer)] = new FunctionCallAnswerSerializer();
        }

        private int GetNextCallId()
        {
            lock (client)
            {
                callIdCounter++;
                return callIdCounter;
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

        private void Run()
        {
            while (true)
            {
                var callAnswer = (FunctionCallAnswer)Aether.Read();
                if (callAnswer != null)
                {
                    answers[callAnswer.Id] = callAnswer;
                    answerEvents[callAnswer.Id].Set();
                }
            }
        }

        public T RemoteFunctionCall<T>(string functionName, bool needsAnswer, params object[] args)
        {
            int id = GetNextCallId();

            var call = new FunctionCall
            {
                Id = id,
                FunctionName = functionName,
                NeedsAnwer = needsAnswer,
                FunctionArguments = args
            };

            if (needsAnswer)
            {
                var answerEvent = answerEvents[id] = new AutoResetEvent(false);

                Aether.Write(call);

                answerEvent.WaitOne(); //wait for answer to arrive
                answerEvents.Remove(id);
                answerEvent.Dispose();
                var answer = answers[id];
                answers.Remove(id);
                if (answer.Exception != null)
                {
                    throw new Exception(answer.Exception);
                }
                return (T)answer.ReturnValue;
            }
            else
            {
                Aether.Write(call);
            }
            return default;
        }

        public void Dispose()
        {
            client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
