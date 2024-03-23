using System.Collections.Generic;
using Thorium.Shared.Messages;

namespace Thorium.Shared.FunctionServer.Tcp
{
    public class FunctionCallHandlerTcp
    {
        protected readonly Dictionary<string, ITcpFunctionProvider> functionProviders = [];

        public void AddFunctionProvider(ITcpFunctionProvider functionProvider)
        {
            functionProviders.Add(functionProvider.FunctionName, functionProvider);
        }

        public bool RemoveFunctionProvider(ITcpFunctionProvider functionProvider)
        {
            string name = functionProvider.FunctionName;
            if (functionProviders.TryGetValue(name, out var entry))
            {
                if (entry == functionProvider)
                {
                    functionProviders.Remove(name);
                    return true;
                }
            }
            return false;
        }

        public object HandleFunctionCall(FunctionCall call, FunctionServerTcpClient client)
        {
            if (functionProviders.TryGetValue(call.FunctionName, out var func))
            {
                return func.Execute(client, call.FunctionArguments);
            }
            else
            {
                throw new FunctionNotFoundException();
            }
        }
    }
}
