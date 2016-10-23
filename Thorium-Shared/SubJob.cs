using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public class SubJob : MarshalByRefObject
    {
        public string JobID { get; }
        public string ID { get; }

        public SubJob(string parentJobID)
        {
            JobID = parentJobID;
            //TODO: set ID
        }
    }
}
