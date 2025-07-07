using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Thorium.Shared.Database;
using Thorium.Shared.Database.Models;
using Thorium.Shared.DTOs;
using Thorium.Shared.FunctionServer.Tcp;
using Thorium.Shared.Util;

namespace Thorium.Server.TcpApi.Functions
{
    public class Register : ITcpFunctionProvider
    {
        public string FunctionName => nameof(Register);

        public object Execute(FunctionServerTcpClient client, object[] args)
        {
            return Register_(client, (string)args[0], (string)args[1]);
        }

        static RegisterAnswer Register_(FunctionServerTcpClient client, string id, string name)
        {
            //TODO: authentication

            var dbConn = DI.ServiceProvider.GetRequiredService<DatabaseConnection>();
            using var db = ThoriumServer.GetNewDb();

            Node? node = null;
            if(id != "unknown")
            {
                node = db.Nodes.FirstOrDefault(x => x.Id == id);
            }
            if (node == null)
            {
                node = new Node()
                {
                    Id = DatabaseContext.GetNewId<Node>(),
                    Identifier = name
                };
                id = node.Id;
                db.Nodes.Add(node);
            }
            db.SaveChanges();
            
            var answer = new RegisterAnswer()
            {
                NodeId = id,
                DatabaseHost = dbConn.Host,
                DatabasePort = dbConn.Port,
                DatabaseName = dbConn.Database,
                DatabaseUser = dbConn.User,
                DatabasePassword = dbConn.Password
            };
            return answer;
        }
    }
}
