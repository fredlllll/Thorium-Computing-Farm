using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Client
{
    public abstract class ClientOperation
    {
        public abstract void Execute(int taskNumber);
    }
}
