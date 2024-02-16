using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Messages
{
    public class FunctionCall
    {
        public int Id { get; set; }
        public string FunctionName { get; set; }
        public object[] FunctionArguments { get; set; }
        public bool NeedsAnwer { get; set; }
    }
}
