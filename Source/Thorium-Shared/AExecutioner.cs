using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public abstract class AExecutioner
    {
        public LightweightTask Task { get; protected set; }

        public AExecutioner(LightweightTask t) {
            Task = t;
        }

        public abstract void Execute();
    }
}
