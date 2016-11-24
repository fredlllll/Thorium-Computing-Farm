using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Blender
{
    public struct BlenderEngineConfig
    {
        public bool UseGPU;
        public int samples;

        public static BlenderEngineConfig Create(Config data)
        {
            BlenderEngineConfig bec = new BlenderEngineConfig();
            bec.UseGPU = data.GetBool("engineUseGPU");
            bec.samples = data.GetInt("engineSamples");
            return bec;
        }
    }
}
