using NLog;
using System;
using System.Collections.Generic;
using System.IO;
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
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string host;
        private readonly int port;
        private int callIdCounter = 0;
        private TcpClient client;
        private NetworkStream stream;
        private readonly Dictionary<int, AutoResetEvent> answerEvents = [];
        private readonly Dictionary<int, FunctionCallAnswer> answers = [];
        private readonly byte[] handshake;

        private Thread runThread;
        private bool running = false;

        public AetherStream Aether { get; private set; }

        public event EventHandler OnClose;

        public FunctionClientTcp(string host, int port, byte[] handshake)
        {
            this.host = host;
            this.port = port;
            this.handshake = handshake;
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
            if (!running)
            {
                client = new TcpClient();
                client.Connect(host, port);
                stream = client.GetStream();
                if (!HandshakeSuccessful())
                {
                    client.Close();
                    throw new Exception("Handshake unsuccessful");
                }
                else
                {
                    Aether = new AetherStream(stream);
                    Aether.Serializers[typeof(FunctionCall)] = new FunctionCallSerializer();
                    Aether.Serializers[typeof(FunctionCallAnswer)] = new FunctionCallAnswerSerializer();

                    runThread = new Thread(Run);
                    running = true;
                    runThread.Start();
                }
            }
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
                runThread.Interrupt();
                runThread.Join();
                OnClose?.Invoke(this, null);
            }
        }

        private void Run()
        {
            while (running)
            {
                FunctionCallAnswer callAnswer;
                try
                {
                    callAnswer = (FunctionCallAnswer)Aether.Read();
                }
                catch (IOException)
                {
                    lock (answers)
                    {
                        answers.Clear();
                    }
                    lock (answerEvents)
                    {
                        foreach (var kv in answerEvents)
                        {
                            kv.Value.Dispose();
                        }
                        answerEvents.Clear();
                    }
                    OnClose?.Invoke(this, null);
                    break;
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                if (callAnswer != null)
                {
                    lock (answerEvents)
                    {
                        if (answerEvents.ContainsKey(callAnswer.Id))
                        {
                            lock (answers)
                            {
                                answers[callAnswer.Id] = callAnswer;
                            }
                            answerEvents[callAnswer.Id].Set();
                        }
                    }
                }
            }
            lock (answers)
            {
                answers.Clear();
            }
            lock (answerEvents)
            {
                foreach (var kv in answerEvents)
                {
                    kv.Value.Dispose();
                }
                answerEvents.Clear();
            }
        }

        public T RemoteFunctionCall<T>(string functionName, bool needsAnswer, int timeoutMs = 5000, params object[] args)
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
                var answerEvent = new AutoResetEvent(false);
                lock (answerEvents)
                {
                    answerEvents[id] = answerEvent;
                }

                Aether.Write(call);

                var start = DateTime.UtcNow;
                try
                {
                    while (!answerEvent.WaitOne(100, true))
                    {
                        if ((DateTime.UtcNow - start).TotalMilliseconds > timeoutMs)
                        {
                            throw new TimeoutException();
                        }
                        //wait for answer to arrive
                    }
                }
                catch (ObjectDisposedException)
                {
                    throw new TimeoutException(); //happens when connection is lost
                }
                lock (answerEvents)
                {
                    answerEvents.Remove(id);
                }
                answerEvent.Dispose();
                FunctionCallAnswer answer;
                lock (answers)
                {
                    answer = answers[id];
                    answers.Remove(id);
                }
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
