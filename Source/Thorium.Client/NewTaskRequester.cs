using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Thorium.Shared;
using Thorium.Threading;

namespace Thorium.Client
{
    public class NewTaskRequester : RestartableThreadClass
    {
        private readonly ThoriumClient thoriumClient;
        private readonly ServerInterface serverInterface;

        public NewTaskRequester(ThoriumClient thoriumClient, ServerInterface serverInterface) : base(true)
        {
            this.thoriumClient = thoriumClient;
            this.serverInterface = serverInterface;
        }

        protected override void Run()
        {
            bool running = true;
            while(running)
            {
                try
                {
                    LightweightTask task = serverInterface.InvokeCheckoutTask();
                    if(task != null)
                    {
                        thoriumClient.AssignTask(task);
                    }
                    while(thoriumClient.CurrentTask != null)
                    {
                        Thread.Sleep(1000);
                    }

                }
                catch(ThreadInterruptedException)
                {
                    running = false;
                    break;
                }
            }
        }
    }
}
