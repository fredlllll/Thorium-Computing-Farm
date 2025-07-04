using System.Net;
using Thorium.Shared.FunctionServer.Http;

namespace Thorium.Server.HttpApi.Functions
{
    public class Register : IHttpFunctionProvider
    {
        public string FunctionName => "Register";

        public void Execute(HttpListenerContext context)
        {
            //TODO: check authentication header
            /*var data = JsonSerializer.Deserialize<RegisterRequest>(context.Request.InputStream);

            string id;
            Nodes nodes = Nodes.Instance;

            if (data.NodeId == null)
            {
                id = Guid.NewGuid().ToString();
                nodes.InsertNew(id, data.NodeName);
            }
            else
            {
                id = data.NodeId;
                if (!nodes.ExistsById(id))
                {
                    nodes.InsertNew(id, data.NodeName);
                }
            }

            var answer = new RegisterAnswer();
            answer.NodeId = id;
            answer.DatabaseHost = Settings.Get<string>("postgresHost");
            answer.DatabasePort = Settings.Get<int>("postgresPort");
            answer.DatabaseName = Settings.Get<string>("postgresMaintenanceDatabase");
            answer.DatabaseUser = Settings.Get<string>("postgresUser");
            answer.DatabasePassword = Settings.Get<string>("postgresPassword");

            JsonSerializer.Serialize(context.Response.OutputStream, answer);*/
            context.Response.OutputStream.Close();
        }
    }
}
