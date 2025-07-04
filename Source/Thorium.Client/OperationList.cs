using System;
using System.Collections.Generic;
using Thorium.Client.Operations;
using Thorium.Shared.Database.Models;

namespace Thorium.Client
{
    public class OperationList
    {
        static readonly Dictionary<string, Type> operationTypes = new();

        static OperationList()
        {
            operationTypes["exe"] = typeof(Exe);
        }

        public readonly List<ClientOperation> operations = new();

        public OperationList(Job job)
        {
            foreach (var operation in job.Operations)
            {
                if (operationTypes.TryGetValue(operation.Type, out var type))
                {
                    operations.Add((ClientOperation)Activator.CreateInstance(type, operation.Data));
                }
                else
                {
                    //TODO: error or dummy operation?
                }
            }
        }
    }
}
