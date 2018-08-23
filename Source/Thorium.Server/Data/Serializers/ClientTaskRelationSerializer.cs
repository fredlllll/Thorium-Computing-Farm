using Thorium.Data;
using Thorium.Data.Implementation.Serializers;

namespace Thorium.Server.Data.Serializers
{
    public class ClientTaskRelationSerializer : BaseSerializer<string, ClientTaskRelation>
    {
        private readonly ClientSerializer clientSer;
        private readonly TaskDataSerializer taskSer;

        public override ARawDatabaseFactory DatabaseFactory { get; }
        string table = null;
        public override string Table
        {
            get
            {
                if(table == null)
                {
                    table = DatabaseFactory.GetDatabase().GetTableName("client_task_relations");
                }
                return table;
            }
        }
        public override string KeyColumn => "client_id";

        public ClientTaskRelationSerializer(ARawDatabaseFactory database, ClientSerializer clientSer, TaskDataSerializer taskSer)
        {
            DatabaseFactory = database;
            this.clientSer = clientSer;
            this.taskSer = taskSer;
        }

        public override ClientTaskRelation Load(string key)
        {
            string client;
            string task;
            using(var reader = SelectStarWhereKey(key))
            {
                reader.Read();

                client = (string)reader["client_id"];
                task = (string)reader["task_id"];
            }
            return new ClientTaskRelation(client, task);
        }

        public override void Save(string key, ClientTaskRelation value)
        {
            string sql = "INSERT INTO " + Table + "(" + KeyColumn + ",client_id,task_id) VALUES(@0,@1,@2) ON DUPLICATE KEY UPDATE client_id=@3,task_id=@4";
            DatabaseFactory.GetDatabase().ExecuteNonQueryTransaction(sql, key, value.Client, value.Task, value.Client, value.Task);
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `task_id` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`" + KeyColumn + @"`) USING BTREE,
  UNIQUE KEY `task_id` (`task_id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";

            DatabaseFactory.GetDatabase().ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {
            string sql = @"ALTER TABLE `" + Table + @"`
  ADD CONSTRAINT `" + Table + @"_ibfk_1` FOREIGN KEY (`" + KeyColumn + @"`) REFERENCES `" + clientSer.Table + @"` (`" + clientSer.KeyColumn + @"`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `" + Table + @"_ibfk_2` FOREIGN KEY (`task_id`) REFERENCES `" + taskSer.Table + @"` (`" + taskSer.KeyColumn + @"`) ON DELETE CASCADE ON UPDATE CASCADE;";

            DatabaseFactory.GetDatabase().ExecuteNonQueryTransaction(sql);
        }
    }
}
