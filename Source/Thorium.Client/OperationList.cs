using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Thorium.Client.Operations;
using Thorium.Shared.Database;
using Thorium.Shared.Database.Models;
using Thorium.Shared.Util;

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
            using var db = ThoriumClient.GetNewDb();
            foreach (var operation in db.Operations.Where(x => x.JobId == job.Id).OrderBy(x => x.OperationIndex).ToList())
            {
                if (operationTypes.TryGetValue(operation.Type, out var type))
                {
                    var op = Activator.CreateInstance(type, operation.Data) as ClientOperation;
                    if (op == null)
                    {
                        throw new Exception("operation could not be instantiated");
                    }
                    operations.Add(op);
                }
                else
                {
                    throw new Exception("operation type " + operation.Type + " does not exist");
                }
            }
        }
    }
}
