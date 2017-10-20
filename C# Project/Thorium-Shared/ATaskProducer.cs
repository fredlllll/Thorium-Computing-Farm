using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public abstract class ATaskProducer
    {
        public AJob Job { get; protected set; }

        public ATaskProducer(AJob job)
        {
            Job = job;
        }

        public abstract Task GetNextTask();
    }
}
