using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codolith.Config;

namespace Thorium_Shared.Blender
{
    public struct BlenderEngineConfig
    {
        public bool UseGPU;
        public int samples;

        public static BlenderEngineConfig Create(Config data)
        {
            BlenderEngineConfig bec = new BlenderEngineConfig();
            bec.UseGPU = data.Get<bool>("engineUseGPU");
            bec.samples = data.Get<int>("engineSamples");
            return bec;
        }
    }
}
