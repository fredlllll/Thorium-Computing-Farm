using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Data.Serializers
{
    public class TaskSerializer : BaseSerializer<string, Task>
    {
        private readonly JobSerializer jobSer;

        public override IDatabase Database { get; }

        public TaskSerializer(IDatabase database, JobSerializer jobSerializer)
        {
            Database = database;
            jobSer = jobSerializer;
        }

        public override string Table => Database.GetTableName("tasks");
        public override string KeyColumn => "id";

        public override Task Load(string key)
        {
            var reader = SelectStarWhereKey(key);
            reader.Read();

            string jobId = (string)reader["job_id"];
            string informationString = (string)reader["information"];
            string status = (string)reader["status"];

            //Job job = jobSer.LoadOrCached(jobId);

            return new Task(jobId, key, JObject.Parse(informationString), (TaskStatus)Enum.Parse(typeof(TaskStatus), status));
        }

        public override void Save(string key, Task value)
        {
            string sql = "INSERT INTO " + Table + "(" + KeyColumn + ",job_id,information,status) VALUES(@0,@1,@2,@3) ON DUPLICATE KEY UPDATE job_id=@4, information=@5, status=@6";
            string informationString = value.Information.ToString(Newtonsoft.Json.Formatting.None);
            Database.ExecuteNonQueryTransaction(sql, key, value.JobID, informationString, value.Status.ToString(), value.JobID, informationString, value.Status.ToString());
        }

        public Task CheckoutTask()
        {
            string sql = "SET @update_id:='';" +
                "UPDATE " + Table + " SET status='" + TaskStatus.Processing.ToString() + "', " + KeyColumn + "=(SELECT @update_id:=" + KeyColumn + ") WHERE status='" + TaskStatus.Waiting.ToString() + "' LIMIT 1;" +
                "SELECT @update_id;";
            var reader = Database.ExecuteQuery(sql);
            if(reader.Read())
            {
                string id = (string)reader[0];
                if(!string.IsNullOrWhiteSpace(id))
                {
                    return Load(id);
                }
            }
            return null;
        }

        public void UpdateStatus(string key, TaskStatus status)
        {
            string sql = "UPDATE " + Table + " SET status=@0 WHERE " + KeyColumn + "=@1";
            Database.ExecuteNonQueryTransaction(sql, status.ToString(), key);
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `job_id` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `information` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` enum('Waiting','Processing','Failed','Finished') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Waiting',
  PRIMARY KEY (`" + KeyColumn + @"`),
  KEY `job_id` (`job_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
";
            Database.ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {
            string sql = @"ALTER TABLE `" + Table + @"`
  ADD CONSTRAINT `" + Table + @"_ibfk_1` FOREIGN KEY (`job_id`) REFERENCES `" + jobSer.Table + @"` (`" + jobSer.KeyColumn + "`) ON DELETE CASCADE ON UPDATE CASCADE;";
            Database.ExecuteNonQueryTransaction(sql);
        }
    }
}
