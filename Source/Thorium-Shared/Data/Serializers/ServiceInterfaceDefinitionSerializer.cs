using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Thorium_Shared.Services;

namespace Thorium_Shared.Data.Serializers
{
    public class ServiceInterfaceDefinitionSerializer : BaseSerializer<string, ServiceInterfaceDefinition>
    {
        public override IDatabase Database { get; }

        public ServiceInterfaceDefinitionSerializer(IDatabase database)
        {
            Database = database;
        }

        public override string Table => Database.GetTableName("service_interface_definitions");

        public override string KeyColumn => "id";

        public override ServiceInterfaceDefinition Load(string key)
        {
            using(var reader = SelectStarWhereKey(key))
            {
                reader.Read();

                string name = (string)reader["name"];
                string interfaceInfoJson = (string)reader["interface_info"];
                var interfaceInfo = JObject.Parse(interfaceInfoJson);

                return new ServiceInterfaceDefinition(key, name, interfaceInfo);
            }
        }

        public override void Save(string key, ServiceInterfaceDefinition value)
        {
            SqlValue[] values = new SqlValue[]
            {
                new SqlValue(KeyColumn,key),
                new SqlValue("name",value.Name),
                new SqlValue("interface_info",value.InterfaceInfo.ToString()),
            };
            string sql = SqlBuilder.InsertOrUpdate(Table, values);
            Database.ExecuteNonQueryTransaction(sql, values.Concat(values).Select(x => x.Value).ToArray());
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `service_definition_id` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `interface_info` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`" + KeyColumn + @"`) USING BTREE,
  KEY `service_definition_id` (`service_definition_id`),
  KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";
            Database.ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {
            string sql = @"ALTER TABLE `" + Table + @"`
  ADD CONSTRAINT `" + Table + @"_ibfk_1` FOREIGN KEY (`service_definition_id`) REFERENCES `" + Database.GetTableName("service_definitions") + @"` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;";
        }
    }
}
