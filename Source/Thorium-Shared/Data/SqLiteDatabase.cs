using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Data
{
    public class SqLiteDatabase : IDatabase
    {
        SQLiteConnection connection;

        public SqLiteDatabase(string dbFile)
        {
            if(!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }
            connection = new SQLiteConnection("Data Source=" + dbFile + ";Version=3;");
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        /// <summary>
        /// executes a query using a transaction. parameter names have to be in the form of @0, @1, @2 ....
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>the last insert id</returns>
        public long ExecuteNonQueryTransaction(string sql, params object[] parameters)
        {
            using(var cmd = new SQLiteCommand(connection))
            using(var transaction = connection.BeginTransaction())
            {
                try
                {
                    cmd.CommandText = sql;
                    for(int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        var sqlParam = new SQLiteParameter("@" + i.ToString(), Util.TypeMap[param.GetType()])
                        {
                            Value = param
                        };
                        cmd.Parameters.Add(sqlParam);
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
            return connection.LastInsertRowId;
        }

        /// <summary>
        /// executes a query and returns its reader. parameter names have to be in the form of @0, @1, @2 ....
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public DbDataReader ExecuteQuery(string sql, params object[] parameters)
        {
            using(var cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = sql;
                for(int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var sqlParam = new SQLiteParameter("@" + i.ToString(), Util.TypeMap[param.GetType()])
                    {
                        Value = param
                    };
                    cmd.Parameters.Add(sqlParam);
                }
                return cmd.ExecuteReader();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                connection.Dispose();
            }
        }
    }
}
