using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server
{
    public class Job
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<FrameBounds> Frames { get; } = new List<FrameBounds>();
        public BackendConfig Config { get; set; }
    }
}
