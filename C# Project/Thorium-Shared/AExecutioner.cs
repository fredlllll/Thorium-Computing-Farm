using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public abstract class AExecutioner
    {
        public Task Task { get; protected set; }

        public AExecutioner(Task t) {
            Task = t;
        }

        public abstract void Execute();
    }
}
