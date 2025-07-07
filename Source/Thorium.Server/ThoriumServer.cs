using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Thorium.Server.HttpApi;
using Thorium.Server.TcpApi;
using Thorium.Shared.Database;
using Thorium.Shared.Database.Models;
using Thorium.Shared.Util;

namespace Thorium.Server
{
    public class ThoriumServer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ThoriumServerHttpApi httpApi = new();
        private readonly ThoriumServerTcpApi tcpApi = new();

        public static DatabaseContext GetNewDb()
        {
            var conn = DI.ServiceProvider.GetRequiredService<DatabaseConnection>();
            var options = DatabaseContext.GetOptionsBuilder(conn.GetConnectionString()).Options;
            return new DatabaseContext(options);
        }

        public void Start()
        {
            logger.Info("Preparing database connection");

            var conn = DatabaseConnection.LoadFromSettings();
            DI.Services.AddSingleton(conn);
            DI.ResetServiceProvider();

            using var db = GetNewDb();
            db.Database.Migrate();


            logger.Info("Database connection established");

            httpApi.Start();
            tcpApi.Start();
        }

        public void Run()
        {
            while (true)
            {
                using var db = GetNewDb();
                //TODO: repeatedly check db for queued jobs and assign them to nodes
                var unassignedTasks = new Queue<Task>(db.Tasks.Where(x => x.Status == TaskStatus.Queued && x.LinedUpOnNodeId == null).AsEnumerable());

                var nodesWithTasksLinedUp = db.Nodes.GroupJoin(
                    db.Tasks,
                    node => node.Id,
                    task => task.LinedUpOnNodeId,
                    (node, tasks) => new
                    {
                        Node = node,
                        NumTasksAssigned = tasks.Take(3).Count()
                    }
                ).ToList();
                int numNodesWithNewTasks = 0;
                foreach (var data in nodesWithTasksLinedUp)
                {
                    if (unassignedTasks.Count <= 0)
                    {
                        break;
                    }
                    if (data.NumTasksAssigned >= 3)
                    {
                        continue; //only assign up to 3 in advance
                    }
                    var task = unassignedTasks.Dequeue();
                    task.LinedUpOnNodeId = data.Node.Id;
                    numNodesWithNewTasks++;
                }
                db.SaveChanges();
                if (numNodesWithNewTasks == 0)
                {
                    //only sleep if no tasks got assigned
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
