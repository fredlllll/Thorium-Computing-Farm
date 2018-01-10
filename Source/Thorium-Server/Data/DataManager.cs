using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Thorium_Server.Data.Serializers;
using Thorium_Shared.Data;
using Thorium_Shared.Data.Serializers;

namespace Thorium_Server.Data
{
    public class DataManager
    {
        public static MySqlConnection GetNewConnection(string host, UInt16 port, string user, string password, string db)
        {
            //reference https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html
            return new MySqlConnection("SERVER=" + host + ";PORT=" + port + ";DATABASE=" + db + ";USER=" + user + ";PASSWORD=" + password + ";");
        }

        public static MySqlDatabase GetNewDatabase()
        {
            dynamic config = Thorium_Shared.Config.ConfigFile.GetConfig("mysql");

            string host = config.DatabaseHost;
            ushort port = config.DatabasePort;
            string user = config.DatabaseUser;
            string password = config.DatabasePassword;
            string db =config.DatabaseName;
            string tablePrefix = config.TablePrefix;

            var conn = GetNewConnection(host, port, user, password, db);
            conn.Open();

            return new MySqlDatabase(conn)
            {
                TablePrefix = tablePrefix
            };
        }

        public TaskSerializer TaskSerializer { get; set; }
        public JobSerializer JobSerializer { get; set; }
        public ClientSerializer ClientSerializer { get; set; }
        public ClientTaskRelationSerializer ClientTaskRelationSerializer { get; set; }

        MySqlDatabase database;

        public DataManager()
        {
            database = GetNewDatabase();

            JobSerializer = new JobSerializer(database);
            TaskSerializer = new TaskSerializer(database, JobSerializer);
            ClientSerializer = new ClientSerializer(database);
            ClientTaskRelationSerializer = new ClientTaskRelationSerializer(database, ClientSerializer, TaskSerializer);
        }
    }
}
