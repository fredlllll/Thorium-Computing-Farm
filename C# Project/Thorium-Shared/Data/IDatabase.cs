using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Data
{
    public interface IDatabase
    {
        void Open();
        void Close();

        long ExecuteNonQueryTransaction(string sql, params object[] parameters);
        DbDataReader ExecuteQuery(string sql, params object[] parameters);
    }
}
