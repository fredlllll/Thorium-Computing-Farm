using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Thorium.Shared.Aether.AetherSerializers;
using Thorium.Shared.Aether;
using Thorium.Shared.Messages;
using System.Threading;
using NLog;

namespace Thorium.Shared.FunctionServer.Tcp
{
    public class FunctionServerTcpClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly FunctionServerTcp server;
        private readonly TcpClient client;
        private bool running = false;
        private NetworkStream stream;
        private AetherStream aether;

        private Thread runThread;

        public string Name { get; set; } = Guid.NewGuid().ToString();

        public FunctionServerTcpClient(FunctionServerTcp server, TcpClient client)
        {
            this.server = server;
            this.client = client;
        }

        public void Start()
        {
            if (!running)
            {
                stream = client.GetStream();
                aether = new AetherStream(stream);
                aether.Serializers[typeof(FunctionCall)] = new FunctionCallSerializer();
                aether.Serializers[typeof(FunctionCallAnswer)] = new FunctionCallAnswerSerializer();

                runThread = new Thread(Run);
                running = true;
                runThread.Start();
            }
        }

        private void CloseOnAway()
        {
            running = false;
            logger.Info("Client " + Name + " has gone away");
            server.SelfRemoveClient(this);
        }

        private void SendAnswer(int id, object result, Exception exception)
        {
            var answer = new FunctionCallAnswer
            {
                Id = id,
                ReturnValue = result,
                Exception = exception?.ToString()
            };
            try
            {
                aether.Write(answer);
            }
            catch (IOException)
            {
                CloseOnAway();
            }
        }

        private void Run()
        {
            while (running)
            {
                FunctionCall call;
                try
                {
                    call = (FunctionCall)aether.Read();
                }
                catch (IOException)
                {
                    CloseOnAway();
                    break;
                }
                object result = null;
                Exception exception = null;
                try
                {
                    try
                    {
                        result = server.FunctionCallHandler.HandleFunctionCall(call, this);
                    }
                    catch (Exception e) when (e is not FunctionNotFoundException)
                    {
                        exception = e;
                    }
                    if (call.NeedsAnwer)
                    {
                        SendAnswer(call.Id, result, exception);
                    }
                }
                catch (FunctionNotFoundException)
                {
                    logger.Error("got call for unknown function " + call.FunctionName);
                    //TODO: probably close connection
                }
            }
        }
    }
}
