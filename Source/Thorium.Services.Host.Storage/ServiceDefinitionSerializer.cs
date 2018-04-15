using System.Linq;
using Thorium_Services_Shared;
using Thorium_Data;
using Thorium_Data_Impl.Serializers;
using Thorium_Data_Impl;

namespace Thorium_Services_Host_Storage
{
    public class ServiceDefinitionSerializer : BaseSerializer<string, ServiceDefinition>
    {
        public override string Table => Database.GetTableName("service_definitions");

        public override string KeyColumn => "id";

        public override IRawDatabase Database { get; }

        public ServiceInterfaceDefinitionSerializer ServiceInterfaceDefinitionSerializer { get; set; }

        public ServiceDefinitionSerializer(IRawDatabase database)
        {
            Database = database;
        }

        public override ServiceDefinition Load(string key)
        {
            string name;
            using(var reader = SelectStarWhereKey(key))
            {
                reader.Read();

                name = (string)reader["name"];
            }

            var interfaceDefinitions = ServiceInterfaceDefinitionSerializer.LoadWhere(new SqlCondition("service_definition_id", key)).ToArray();

            return new ServiceDefinition(key, name, interfaceDefinitions);
        }

        public override void Save(string key, ServiceDefinition value)
        {
            SqlValue[] values = new SqlValue[]
            {
                new SqlValue(KeyColumn,key),
                new SqlValue("name",value.Name),
            };
            string sql = SqlBuilder.InsertOrUpdate(Table, values);
            Database.ExecuteNonQueryTransaction(sql, values.Concat(values).Select(x => x.Value).ToArray());

            foreach(var sid in value.InterfaceDefinitions)
            {
                ServiceInterfaceDefinitionSerializer.Save(sid.Id, sid);
            }
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY(`" + KeyColumn + @"`),
  KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";
            Database.ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {
        }
    }
}
