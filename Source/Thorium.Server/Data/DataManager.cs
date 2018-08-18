using System;
using MySql.Data.MySqlClient;
using Thorium.Config;
using Thorium.Server.Data.Serializers;
using Thorium.Data.Implementation;
using Thorium.Data.Implementation.Serializers;

namespace Thorium.Server.Data
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
            dynamic config = ConfigFile.GetConfig("mysql");

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

        public TaskDataSerializer TaskSerializer { get; set; }
        public ClientSerializer ClientSerializer { get; set; }
        public ClientTaskRelationSerializer ClientTaskRelationSerializer { get; set; }

        MySqlDatabase database;

        public DataManager()
        {
            database = GetNewDatabase();

            TaskSerializer = new TaskDataSerializer(database);
            ClientSerializer = new ClientSerializer(database);
            ClientTaskRelationSerializer = new ClientTaskRelationSerializer(database, ClientSerializer, TaskSerializer);
        }
    }
}
