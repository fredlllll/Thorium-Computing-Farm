using System.Linq;
using System.Text;

namespace Thorium_Data_Impl
{
    public static class SqlBuilder
    {
        public static string SelectFrom(string[] whats, string table, params SqlCondition[] conditions)
        {
            return SelectFrom(whats, table, conditions.Select(x => x.Column).ToArray());
        }

        public static string SelectFrom(string[] whats, string table, string[] whereColumns)
        {
            StringBuilder sql = new StringBuilder("SELECT " + string.Join(",", whats) + " FROM " + table);
            if(whereColumns.Length > 0)
            {
                sql.Append(" WHERE");
                for(int i = 0; i < whereColumns.Length; i++)
                {
                    var column = whereColumns[i];
                    sql.Append(" ");
                    sql.Append(column);
                    sql.Append(" = ");
                    sql.Append("@" + i);
                    if(i < whereColumns.Length - 1)
                    {
                        sql.Append(" AND ");
                    }
                }
            }
            sql.Append(";");
            return sql.ToString();
        }

        public static string InsertOrUpdate(string table, params SqlValue[] values)
        {
            return InsertOrUpdate(table, values.Select(x => x.Column).ToArray());
        }

        public static string InsertOrUpdate(string table, string[] columns)
        {
            StringBuilder sql = new StringBuilder("INSERT INTO " + table + "(");
            sql.Append(string.Join(",", columns));
            sql.Append(") VALUES(");
            for(int i = 0; i < columns.Length; i++)
            {
                sql.Append("@" + i);
                if(i < columns.Length - 1)
                {
                    sql.Append(",");
                }
            }
            sql.Append(") ON DUPLICATE KEY UPDATE ");
            for(int i = 0; i < columns.Length; i++)
            {
                sql.Append(columns[i]);
                sql.Append("=@" + (columns.Length) + i);
                if(i < columns.Length - 1)
                {
                    sql.Append(",");
                }
            }
            sql.Append(";");
            return sql.ToString();
        }

        public static string DeleteWhere(string table, params SqlCondition[] conditions)
        {
            return DeleteWhere(table, conditions.Select(x => x.Column).ToArray());
        }

        public static string DeleteWhere(string table, string[] whereColumns)
        {
            StringBuilder sql = new StringBuilder("DELETE FROM " + table);
            sql.Append(" WHERE");
            for(int i = 0; i < whereColumns.Length; i++)
            {
                var column = whereColumns[i];
                sql.Append(" ");
                sql.Append(column);
                sql.Append(" = ");
                sql.Append("@" + i);
                if(i < whereColumns.Length - 1)
                {
                    sql.Append(" AND ");
                }
            }
            sql.Append(";");
            return sql.ToString();
        }
    }
}
