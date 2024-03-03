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

namespace Thorium.Shared
{
    public class FunctionServerTcpClient
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly FunctionServerTcp server;
        private readonly TcpClient client;
        private readonly NetworkStream stream;
        private readonly AetherStream aether;
        private readonly Dictionary<int, AutoResetEvent> answerEvents = new();
        private readonly Dictionary<int, FunctionCallAnswer> answers = new();

        private Thread runThread;

        public FunctionServerTcpClient(FunctionServerTcp server, TcpClient client)
        {
            this.server = server;
            this.client = client;
            stream = client.GetStream();
            aether = new AetherStream(stream);
            aether.Serializers[typeof(FunctionCall)] = new FunctionCallSerializer();
            aether.Serializers[typeof(FunctionCallAnswer)] = new FunctionCallAnswerSerializer();

            runThread = new Thread(Run);
            runThread.Start();
        }

        private void SendAnswer(int id, object result, Exception exception)
        {
            var answer = new FunctionCallAnswer();
            answer.Id = id;
            answer.ReturnValue = result;
            answer.Exception = exception?.ToString();
            aether.Write(answer);
        }

        private void Run()
        {
            while (true)
            {
                var call = (FunctionCall)aether.Read();
                object result = null;
                Exception exception = null;
                try
                {
                    try
                    {
                        result = server.HandleFunctionCall(call, this);
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
