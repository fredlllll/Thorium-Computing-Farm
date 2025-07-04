using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Thorium.Server.HttpApi;
using Thorium.Server.TcpApi;
using Thorium.Shared.Database;
using Thorium.Shared.Util;

namespace Thorium.Server
{
    public class ThoriumServer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ThoriumServerHttpApi httpApi = new();
        private readonly ThoriumServerTcpApi tcpApi = new();

        public void Start()
        {
            logger.Info("Preparing database connection");

            var conn = DatabaseConnection.LoadFromSettings();
            DI.Services.AddSingleton(conn);

            var options = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(conn.GetConnectionString()).Options;
            DatabaseContext db = new DatabaseContext(options);
            DI.Services.AddSingleton(db);
            db.Database.Migrate();


            logger.Info("Database connection established");

            httpApi.Start();
            tcpApi.Start();
        }

        public void Run()
        {
            while (true)
            {
                //TODO: repeatedly check db for queued jobs and assign them to nodes
                Thread.Sleep(1000);
            }
        }
    }
}
