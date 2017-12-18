using System.Collections.Generic;
using System.Data.Common;

namespace Thorium_Shared.Data.Serializers
{
    public abstract class BaseSerializer<TKey, TValue> : ISerializer<TKey, TValue>
    {
        public abstract IDatabase Database { get; }
        public abstract string Table { get; }
        public abstract string KeyColumn { get; }

        public abstract void CreateConstraints();
        public abstract void CreateTable();
        public abstract TValue Load(TKey key);
        public abstract void Save(TKey key, TValue value);

        public void Delete(TKey key)
        {
            string sql = "DELETE FROM " + Table + " WHERE " + KeyColumn + "=@0;";
            Database.ExecuteNonQueryTransaction(sql, key);
        }

        public DbDataReader SelectStarWhereKey(TKey key)
        {
            string sql = "SELECT * FROM " + Table + " WHERE " + KeyColumn + " = @0;";
            DbDataReader reader = Database.ExecuteQuery(sql, key);
            if(!reader.HasRows)
            {
                throw new KeyNotFoundException("no element with key '" + key + "' found in database");
            }
            return reader;
        }

        public IEnumerable<TValue> LoadAll()
        {
            string sql = "SELECT " + KeyColumn + " FROM " + Table;
            List<TKey> keys = new List<TKey>();
            using(var reader = Database.ExecuteQuery(sql))
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        keys.Add((TKey)reader[KeyColumn]);
                    }
                }
            }
            foreach(var key in keys)
            {
                yield return Load(key);
            }
        }

        public IEnumerable<TValue> LoadWhere<TWhere>(string column, TWhere whereis)
        {
            string sql = "SELECT " + KeyColumn + " FROM " + Table + " WHERE " + column + " = @0;";
            List<TKey> keys = new List<TKey>();
            using(var reader = Database.ExecuteQuery(sql, whereis))
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        keys.Add((TKey)reader[KeyColumn]);
                    }
                }
            }

            foreach(var key in keys)
            {
                yield return Load(key);
            }
        }

        public void DeleteWhere<TWhere>(string column, TWhere whereis)
        {
            string sql = "DELETE FROM " + Table + " WHERE " + column + " = @0";
            Database.ExecuteNonQueryTransaction(sql, whereis);
        }
    }
}
