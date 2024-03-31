using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.Net.Tcp
{
    public class Message
    {
        public long Id { get; set; }
        public object Payload { get; set; }
    }
}
