using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Messages
{
    public class FunctionCallAnswer
    {
        public int Id { get; set; }
        public object ReturnValue { get; set; }
        public string Exception { get; set; }
    }
}
