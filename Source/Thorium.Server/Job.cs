using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thorium.Server
{
    public class Job
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Task[] Tasks {get;set;}

        public Operation[] Operations { get;set;}
    }
}
