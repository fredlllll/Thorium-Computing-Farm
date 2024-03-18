using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using Thorium.Shared.Messages;

namespace Thorium.Shared
{
    public class FunctionServerTcp
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly TcpListener listener;
        private readonly Dictionary<string, Tuple<MethodInfo, object>> functions = [];
        private readonly byte[] handshake;

        private readonly List<FunctionServerTcpClient> clients = [];

        public event EventHandler<TcpClient> ClientHandshakeSucceeded;
        public event EventHandler<TcpClient> ClientHandshakeFailed;

        public FunctionServerTcp(TcpListener listener, byte[] handshake)
        {
            this.listener = listener;
            this.handshake = handshake;
        }

        public void Start()
        {
            listener.Start();
            listener.BeginAcceptTcpClient(AcceptClient, this);
        }

        private bool CheckHandshake(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[handshake.Length];
            stream.ReadTimeout = 1000;
            stream.WriteTimeout = 1000;
            try
            {
                stream.ReadExactly(buffer, 0, buffer.Length);
            }
            catch (IOException)
            {
                return false;
            }

            bool clientHandshake = buffer.SequenceEqual(handshake);
            if (clientHandshake)
            {
                //write handshake back on success
                try
                {
                    stream.Write(handshake, 0, handshake.Length);
                }
                catch (IOException)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private void AcceptClient(IAsyncResult asyncResult)
        {
            var client = listener.EndAcceptTcpClient(asyncResult);

            if (CheckHandshake(client))
            {
                ClientHandshakeSucceeded?.Invoke(this, client);
                clients.Add(new FunctionServerTcpClient(this, client));
            }
            else
            {
                ClientHandshakeFailed?.Invoke(this, client);
                client.Close();
            }

            listener.BeginAcceptTcpClient(AcceptClient, this);
        }

        public void AddFunction(MethodInfo methodInfo, object target)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0 || parameters[0].ParameterType != typeof(FunctionServerTcpClient))
            {
                throw new Exception($"functions need {nameof(FunctionServerTcpClient)} as first argument type");
            }

            functions[methodInfo.Name] = new Tuple<MethodInfo, object>(methodInfo, target);
        }

        public object HandleFunctionCall(FunctionCall call, FunctionServerTcpClient client)
        {
            if (functions.TryGetValue(call.FunctionName, out var func))
            {
                object[] args = call.FunctionArguments;
                if (args.Length > 0)
                {
                    var new_args = new object[args.Length + 1];
                    new_args[0] = client;
                    for (int i = 0; i < args.Length; i++)
                    {
                        new_args[i + 1] = args[i];
                    }
                    args = new_args;
                }
                else
                {
                    args = [client];
                }
                return func.Item1.Invoke(func.Item2, args);
            }
            else
            {
                throw new FunctionNotFoundException();
            }
        }

        internal void SelfRemoveClient(FunctionServerTcpClient client)
        {
            clients.Remove(client);
        }
    }
}
