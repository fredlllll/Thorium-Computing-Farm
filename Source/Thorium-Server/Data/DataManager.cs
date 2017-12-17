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
        public TaskSerializer TaskSerializer { get; set; }
        public JobSerializer JobSerializer { get; set; }
        public ClientSerializer ClientSerializer { get; set; }
        public ClientTaskRelationSerializer ClientTaskRelationSerializer { get; set; }

        MySqlDatabase database;

        public DataManager(string host, UInt16 port, string user, string password, string db, string tablePrefix)
        {
            var conn = new MySqlConnection("");

            database = new MySqlDatabase(conn)
            {
                TablePrefix = tablePrefix
            };

            JobSerializer = new JobSerializer(database);
            TaskSerializer = new TaskSerializer(database, JobSerializer);
            ClientSerializer = new ClientSerializer(database);
            ClientTaskRelationSerializer = new ClientTaskRelationSerializer(database, ClientSerializer, TaskSerializer);
        }
    }
}
