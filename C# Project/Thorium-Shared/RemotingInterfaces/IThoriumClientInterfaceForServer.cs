using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public interface IThoriumClientInterfaceForServer
    {
        string ID { get; }
        string currentTaskID { get; set; }

        void Ping();
        void AbortTask(string ID);
        //void Shutdown();
    }
}
