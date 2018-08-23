using System;
using Newtonsoft.Json.Linq;
using Thorium.Shared;

namespace Thorium.Data.Implementation.Serializers
{
    public class TaskDataSerializer : BaseSerializer<string, TaskData>
    {
        public override ARawDatabaseFactory DatabaseFactory { get; }

        public TaskDataSerializer(ARawDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
        }

        string table = null;
        public override string Table
        {
            get
            {
                if(table == null)
                {
                    table = DatabaseFactory.GetDatabase().GetTableName("tasks");
                }
                return table;
            }
        }
        public override string KeyColumn => "id";

        public override TaskData Load(string key)
        {
            string informationString;
            string status;
            using(var reader = SelectStarWhereKey(key))
            {
                reader.Read();

                informationString = (string)reader["information"];
                status = (string)reader["status"];
            }
            return new TaskData(key, JObject.Parse(informationString), (TaskStatus)Enum.Parse(typeof(TaskStatus), status));
        }

        public override void Save(string key, TaskData value)
        {
            string sql = "INSERT INTO " + Table + "(" + KeyColumn + ",information,status) VALUES(@0,@1,@2) ON DUPLICATE KEY UPDATE information=@3, status=@4";
            string informationString = value.Information.ToString(Newtonsoft.Json.Formatting.None);
            DatabaseFactory.GetDatabase().ExecuteNonQueryTransaction(sql, key, informationString, value.Status.ToString(), informationString, value.Status.ToString());
        }

        public TaskData CheckoutTask()
        {
            string sql = "SET @update_id:='';" +
                "UPDATE " + Table + " SET status='" + TaskStatus.Executing.ToString() + "', " + KeyColumn + "=(SELECT @update_id:=" + KeyColumn + ") WHERE status='" + TaskStatus.WaitingForExecution.ToString() + "' LIMIT 1;" +
                "SELECT @update_id;";

            string id = null;
            using(var reader = DatabaseFactory.GetDatabase().ExecuteQuery(sql))
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    id = (string)reader[0];
                }
            }
            if(!string.IsNullOrWhiteSpace(id))
            {
                return Load(id);
            }
            return null;
        }

        public void UpdateStatus(string key, TaskStatus status)
        {
            string sql = "UPDATE " + Table + " SET status=@0 WHERE " + KeyColumn + "=@1";
            DatabaseFactory.GetDatabase().ExecuteNonQueryTransaction(sql, status.ToString(), key);
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `information` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` enum('Waiting','Processing','Failed','Finished') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Waiting',
  PRIMARY KEY (`" + KeyColumn + @"`),
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
";
            DatabaseFactory.GetDatabase().ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {
            // no constraints
        }
    }
}
