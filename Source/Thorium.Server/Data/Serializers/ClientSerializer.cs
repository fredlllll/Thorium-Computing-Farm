using System.Net;
using Thorium.Data;
using Thorium.Data.Implementation.Serializers;

namespace Thorium.Server.Data.Serializers
{
    public class ClientSerializer : BaseSerializer<string, Client>
    {
        public override IRawDatabase Database { get; }
        public override string Table => Database.GetTableName("clients");
        public override string KeyColumn => "id";

        public ClientSerializer(IRawDatabase database)
        {
            Database = database;
        }

        public override Client Load(string key)
        {
            string ip;
            string status_str;
            using(var reader = SelectStarWhereKey(key))
            {
                reader.Read();

                ip = (string)reader["ip"];
                status_str = (string)reader["status"];
            }
            return new Client(key, IPAddress.Parse(ip), (ClientStatus)System.Enum.Parse(typeof(ClientStatus), status_str));
        }

        public override void Save(string key, Client value)
        {
            string sql = "INSERT INTO " + Table + "(" + KeyColumn + ",ip, status) VALUES(@0,@1,@2) ON DUPLICATE KEY UPDATE ip=@3, status=@4";
            var ip = value.IPAddress.ToString();
            var status = value.Status.ToString();
            Database.ExecuteNonQueryTransaction(sql, key, ip, status, ip, status);
        }

        public Client CheckoutClient()
        {
            string sql = "SET @update_id:='';" +
                "UPDATE " + Table + " SET status='" + ClientStatus.Busy.ToString() + "', " + KeyColumn + "=(SELECT @update_id:=" + KeyColumn + ") WHERE status='" + ClientStatus.Idle.ToString() + "' LIMIT 1;" +
                "SELECT @update_id;";

            string id = null;
            using(var reader = Database.ExecuteQuery(sql))
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

        public void UpdateStatus(string key, ClientStatus status)
        {
            string sql = "UPDATE " + Table + " SET status=@0 WHERE " + KeyColumn + "=@1";
            Database.ExecuteNonQueryTransaction(sql, status.ToString(), key);
        }

        public override void CreateTable()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `" + Table + @"` (
  `" + KeyColumn + @"` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ip` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `status` enum('Idle','Busy') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Idle',
  PRIMARY KEY (`" + KeyColumn + @"`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";
            Database.ExecuteNonQueryTransaction(sql);
        }

        public override void CreateConstraints()
        {
        }
    }
}
