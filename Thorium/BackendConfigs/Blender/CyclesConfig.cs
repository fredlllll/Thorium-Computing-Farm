using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Server.BackendConfigs.Blender
{
    public enum CPUGPUConfig
    {
        CPU,
        GPU,
        CPU_GPU
    }

    public class CyclesConfig : BlenderRenderConfig
    {
        public CPUGPUConfig CPUGPUConfig { get; set; } = CPUGPUConfig.CPU;
    }
}
