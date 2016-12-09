﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;

namespace Thorium_Shared
{
    [ServiceContract]
    public interface IThoriumClientInterfaceForServer : IService
    {
        [OperationContract]
        string GetID();
        [OperationContract]
        string GetCurrentTaskID();
        [OperationContract]
        void SetCurrentTaskID(string id);

        [OperationContract]
        void Ping();
        [OperationContract]
        void AbortTask(string ID);
        //[OperationContract]
        //void Shutdown();
    }
}