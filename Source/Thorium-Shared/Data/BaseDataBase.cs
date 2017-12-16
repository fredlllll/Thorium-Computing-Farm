using System;
using System.Data;
using System.Data.Common;

namespace Thorium_Shared.Data
{
    public abstract class BaseDataBase : IDatabase, IDisposable
    {
        public DbConnection Connection { get; protected set; }

        public string TablePrefix { get; set; } = string.Empty;

        public BaseDataBase(DbConnection connection)
        {
            Connection = connection;
        }

        public virtual string GetTableName(string name)
        {
            return TablePrefix + "_" + name;
        }

        public virtual DbCommand GetCommand()
        {
            return Connection.CreateCommand();
        }

        public virtual DbTransaction GetTransaction()
        {
            return Connection.BeginTransaction();
        }

        public abstract DbParameter GetParameter(int index, DbType type, object value);
        /*
        var sqlParam = new SQLiteParameter("@" + i.ToString(), Util.TypeMap[param.GetType()])
        {
            Value = param
        };
        */

        public virtual void Open()
        {
            Connection.Open();
        }

        public virtual void Close()
        {
            Connection.Close();
        }

        public void ExecuteNonQueryTransaction(string sql, params object[] parameters)
        {
            using(var cmd = GetCommand())
            using(var transaction = GetTransaction())
            {
                try
                {
                    cmd.CommandText = sql;
                    for(int i = 0; i < parameters.Length; i++)
                    {
                        var paramValue = parameters[i];
                        var param = GetParameter(i, Util.TypeMap[paramValue.GetType()], paramValue);
                        cmd.Parameters.Add(param);
                    }
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public DbDataReader ExecuteQuery(string sql, params object[] parameters)
        {
            using(var cmd = GetCommand())
            {
                cmd.CommandText = sql;
                for(int i = 0; i < parameters.Length; i++)
                {
                    var paramValue = parameters[i];
                    var param = GetParameter(i, Util.TypeMap[paramValue.GetType()], paramValue);
                    cmd.Parameters.Add(param);
                }
                return cmd.ExecuteReader();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    Connection.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
