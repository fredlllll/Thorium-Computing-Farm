using System.Collections.Generic;
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
            var reader = SelectStarWhereKey(key);
            reader.Read();

            string name = (string)reader["name"];
            string informationString = (string)reader["information"];

            return new Job(key, name, JObject.Parse(informationString));
        }

        public override void Save(string key, Job value)
        {
            string sql = "INSERT INTO " + Table + "(" + KeyColumn + ",name,information) VALUES(@0,@1,@2) ON DUPLICATE KEY UPDATE name=@3, information=@4";
            string informationString = value.Information.ToString(Newtonsoft.Json.Formatting.None);
            Database.ExecuteNonQueryTransaction(sql, key, value.Name, informationString, value.Name, informationString);
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `"+Table+ @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `name` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `information` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`" + KeyColumn+@"`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";
            Database.ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {

        }
    }
}
