using System;
using System.Collections.Generic;
using System.Text;
using Thorium.Shared;
using Thorium.Threading;

namespace Thorium.Client
{
    public class NewTaskRequester :RestartableThreadClass
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
            while(true)
            {
                LightweightTask task = serverInterface.InvokeCheckoutTask();
                if(task != null)
                {
                    thoriumClient.AssignTask(task);
                }
            }
        }
    }
}
