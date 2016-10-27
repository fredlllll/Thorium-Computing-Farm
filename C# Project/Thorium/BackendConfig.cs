using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public abstract class BackendConfig
    {
        public abstract JobPart[] GetAllJobs(Job job);
    }
}
