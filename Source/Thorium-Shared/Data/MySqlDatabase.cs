using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Thorium_Shared.Data
{
    public class MySqlDatabase : BaseDataBase
    {
        public MySqlDatabase(MySqlConnection connection) : base(connection)
        {
        }

        public override DbParameter GetParameter(int index, DbType type, object value)
        {
            return new MySqlParameter("@" + index, type)
            {
                Value = value
            };
        }
    }
}
