using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared;

namespace Thorium_Server.BackendConfigs.Blender
{
    public class BlenderJobPart : JobPart
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public Resolution Resolution { get; set; }
        public string[] Layers { get; set; }
        public string FilePath { get; set; }

        public BlenderJobPart(string parentJobID) : base(parentJobID)
        {
        }

        public override void PopulateJobExecutionInfo(IJobPartExecutionInfo part)
        {
            //TODO: set the 3 actions to render
        }
    }
}
