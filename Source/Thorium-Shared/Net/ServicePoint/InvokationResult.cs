using System;
using Newtonsoft.Json.Linq;

namespace Thorium_Shared.Net.ServicePoint
{
    public class InvokationResult
    {
        public Exception Exception { get; set; }
        public JToken ReturnValue { get; set; }
    }
}
