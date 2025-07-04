using System.Net;
using Thorium.Shared;
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
            logger.Info("Starting http API");
            api.Start();
            logger.Info("Http API listening on port " + Settings.Get<int>("httpApiPort"));
        }

        public void Stop()
        {
            api.Stop();
        }
    }
}
