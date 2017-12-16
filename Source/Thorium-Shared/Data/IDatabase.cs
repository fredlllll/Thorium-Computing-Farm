using System;
using System.Data;
using System.Data.Common;

namespace Thorium_Shared.Data
{
    public interface IDatabase
    {
        void Open();
        void Close();

        DbCommand GetCommand();
        DbTransaction GetTransaction();
        DbParameter GetParameter(int index, DbType type, object value);

        string GetTableName(string name);

        void ExecuteNonQueryTransaction(string sql, params object[] parameters);
        /// <summary>
        /// runs a query and returns its DbDataReader. parameter placeholders have to be in the format of @&lt;0 based index of parameter&gt;
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DbDataReader ExecuteQuery(string sql, params object[] parameters);
    }
}
