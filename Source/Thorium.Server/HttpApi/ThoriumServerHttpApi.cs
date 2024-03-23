using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thorium.Shared;
using Thorium.Shared.DTOs.OperationData;
using Thorium.Shared.DTOs;
using NLog;
using Thorium.Shared.FunctionServer.Http;
using Thorium.Server.HttpApi.Functions;

namespace Thorium.Server.HttpApi
{
    public class ThoriumServerHttpApi
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly FunctionServerHttp api;


        public ThoriumServerHttpApi()
        {
            var apiListener = new HttpListener();
            apiListener.Prefixes.Add("http://*:" + Settings.Get<int>("httpApiPort") + "/");

            api = new FunctionServerHttp(apiListener);

            api.AddFunctionProvider(new AddJob());
        }

        public void Start()
        {
            api.Start();
            logger.Info("Http API listening on port " + Settings.Get<int>("httpApiPort"));
        }

        public void Stop()
        {
            api.Stop();
        }
    }
}
