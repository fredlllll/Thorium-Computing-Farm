using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Data.Serializers
{
    public class JobSerializer : BaseSerializer<string, Job>
    {
        public override IDatabase Database { get; }

        public JobSerializer(IDatabase database)
        {
            Database = database;
        }

        public override string Table => Database.GetTableName("jobs");

        public override string KeyColumn => "id";

        public override Job Load(string key)
        {
            using(var reader = SelectStarWhereKey(key))
            {
                reader.Read();

                string name = (string)reader["name"];
                string informationString = (string)reader["information"];
                JobStatus jobStatus = (JobStatus)Enum.Parse(typeof(JobStatus), (string)reader["status"]);

                return new Job(key, name, JObject.Parse(informationString), jobStatus);
            }
        }

        public override void Save(string key, Job value)
        {
            SqlValue[] values = new SqlValue[]
            {
                new SqlValue(KeyColumn,key),
                new SqlValue("name",value.Name),
                new SqlValue("information", value.Information.ToString()),
                new SqlValue("status",value.Status.ToString())
            };
            string sql = SqlBuilder.InsertOrUpdate(Table, values);
            //string sql = "INSERT INTO " + Table + "(" + KeyColumn + ",name,information,status) VALUES(@0,@1,@2,@3) ON DUPLICATE KEY UPDATE name=@4, information=@5, status=@6";
            //string informationString = value.Information.ToString(Newtonsoft.Json.Formatting.None);
            //Database.ExecuteNonQueryTransaction(sql, key, value.Name, informationString, value.Status.ToString(), value.Name, informationString, value.Status.ToString());
            Database.ExecuteNonQueryTransaction(sql, values.Concat(values).Select(x => x.Value).ToArray());
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `name` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `information` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` enum('Initializing','Initialized') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Initializing',
  PRIMARY KEY (`" + KeyColumn + @"`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";
            Database.ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {

        }
    }
}
