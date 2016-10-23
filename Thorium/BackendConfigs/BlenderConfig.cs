using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Server.BackendConfigs.Blender;
using Thorium_Shared;

namespace Thorium_Server.BackendConfigs
{
    public class BlenderConfig : BackendConfig
    {
        public int TilesX { get; set; }
        public int TilesY { get; set; }
        public Resolution Resolution { get; set; }
        public List<string> Layers { get; } = new List<string>();
        public BlenderRenderConfig Config { get; set; }

        public override void PopulateSubJob(SubJob sj)
        {
            throw new NotImplementedException();
        }
    }
}
