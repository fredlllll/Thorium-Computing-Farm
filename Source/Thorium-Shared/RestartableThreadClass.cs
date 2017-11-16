using System.Threading;

namespace Thorium_Shared
{
    public abstract class RestartableThreadClass
    {
        protected object runThreadLock = new object();
        protected Thread runThread = null;
        bool isBackground;

        public RestartableThreadClass(bool isBackground)
        {
            this.isBackground = isBackground;
        }

        public virtual void Start()
        {
            lock(runThreadLock)
            {
                if(runThread == null)
                {
                    runThread = new Thread(Run) { IsBackground = isBackground };
                }
                if((runThread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
                {
                    runThread.Start();
                }
            }
        }

        public virtual void Stop()
        {
            lock(runThreadLock)
            {
                if(runThread != null)
                {
                    runThread.Interrupt();
                    if(Thread.CurrentThread != runThread)
                    {
                        runThread.Join();
                    }
                    runThread = null;
                }
            }
        }

        protected abstract void Run();
    }
}
