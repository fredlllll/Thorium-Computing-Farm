using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thorium.Client.Operations;
using Thorium.Shared.DTOs;

namespace Thorium.Client
{
    public class OperationList
    {
        static Dictionary<string, Type> operationTypes = new();

        static OperationList()
        {
            operationTypes["exe"] = typeof(Exe);
        }

        public readonly ClientOperation[] operations;

        public OperationList(JobDTO job)
        {
            operations = new ClientOperation[job.Operations.Length];
            for (int i = 0; i < job.Operations.Length; i++)
            {
                var opData = job.Operations[i];
                if (operationTypes.TryGetValue(opData.OperationType, out var type))
                {
                    var shit = opData.OperationData.GetType();
                    var op = Activator.CreateInstance(type, opData.OperationData);
                    operations[i] = (ClientOperation)op;
                }
                else
                {
                    //TODO: error or dummy operation?
                }
            }
        }
    }
}
