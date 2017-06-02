using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    [Serializable]
    public abstract class ATaskInformation
    {
        public delegate void TaskAbortedHandler(ATaskInformation taskInfo);
        public delegate void TaskFinishedHandler(ATaskInformation taskInfo);
        public delegate void TaskCanceledHandler(ATaskInformation taskInfo);

        public event TaskAbortedHandler TaskAborted;
        public event TaskFinishedHandler TaskFinished;
        public event TaskCanceledHandler TaskCanceled;
    }
}
