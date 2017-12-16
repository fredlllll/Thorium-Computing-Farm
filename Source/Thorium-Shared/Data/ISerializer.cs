using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Data
{
    public interface ISerializer<TKey, TValue>
    {
        IDatabase Database { get; }
        string Table { get; }
        string KeyColumn { get; }

        void CreateTable();
        void CreateConstraints();
        void Save(TKey key, TValue value);
        TValue Load(TKey key);
        void Delete(TKey key);
    }
}
