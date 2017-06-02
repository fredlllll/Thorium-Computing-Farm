using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    /// <summary>
    /// a task creator whose state can be saved and restored
    /// </summary>
    [Serializable]
    public abstract class ATaskInformationProducer
    {
        /// <summary>
        /// returns how many more tasks this producer can provide
        /// </summary>
        public abstract int RemainingTaskInformationCount
        {
            get;
        }
        /// <summary>
        /// returns the next task
        /// </summary>
        /// <returns>next task</returns>
        public abstract ATaskInformation GetNextTaskInformation();
    }
}
