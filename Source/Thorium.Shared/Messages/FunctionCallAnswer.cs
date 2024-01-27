using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.Messages
{
    public class FunctionCallAnswer
    {
        public int Id { get; set; }
        public object ReturnValue {  get; set; }
        public Exception Exception { get; set; }
    }
}
