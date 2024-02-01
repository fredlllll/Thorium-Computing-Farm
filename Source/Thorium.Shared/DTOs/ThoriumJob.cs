using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.DTOs
{
    public class ThoriumJob
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int TaskCount { get; set; }

        public ClientOperation[] Operations { get; set; }
        //TODO: all the other things
    }
}
