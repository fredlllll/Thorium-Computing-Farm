﻿using System.Threading;
using NLog;
using Thorium.Server.HttpApi;
using Thorium.Server.TcpApi;

namespace Thorium.Server
{
    public class ThoriumServer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ThoriumServerTcpApi tcpApi = new();
        private readonly ThoriumServerHttpApi httpApi = new();

        public void Start()
        {
            tcpApi.Start();
            httpApi.Start();
        }

        public void Run()
        {
            while (true)
            {
                //TODO: i dont really know what to even do here
                Thread.Sleep(1000);
            }
        }
    }
}
