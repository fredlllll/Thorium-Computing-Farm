using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Thorium_Data;

namespace Thorium_Data_Impl
{
    public class MySqlDatabase : IRawDatabase, IDisposable
    {
        public MySqlDatabase(MySqlConnection connection)
        {
            Connection = connection;
        }

        public DbConnection Connection { get; protected set; }

        public string TablePrefix { get; set; } = string.Empty;

        public virtual string GetTableName(string name)
        {
            return TablePrefix + "_" + name;
        }

        public DbCommand GetCommand()
        {
            return Connection.CreateCommand();
        }

        public DbTransaction GetTransaction()
        {
            return Connection.BeginTransaction();
        }

        public DbParameter GetParameter(DbCommand cmd, int index, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = "@" + index;
            parameter.Value = value;
            return parameter;
            //return new MySqlParameter("@" + index, value);
        }

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
                        var param = GetParameter(cmd, i, paramValue);
                        cmd.Parameters.Add(param);
                    }
                    cmd.Prepare();
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
            var cmd = GetCommand(); //cmd is closed from reader
            cmd.CommandText = sql;
            for(int i = 0; i < parameters.Length; i++)
            {
                var paramValue = parameters[i];
                var param = GetParameter(cmd, i, paramValue);
                cmd.Parameters.Add(param);
            }
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            return reader;
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
