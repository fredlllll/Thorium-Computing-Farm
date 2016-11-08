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
        public string[] Layers { get; set; }
        public string FilePath { get; set; }
        public BlenderRenderConfig Config { get; set; }

        public override Thorium_Shared.Task[] GetAllJobs(Job job)
        {
            List<Thorium_Shared.Task> jobs = new List<Thorium_Shared.Task>();


            foreach(var fb in job.Frames)
            {
                for(int y = 0; y < TilesY; y++)
                {
                    for(int x = 0; x < TilesX; x++)
                    {
                        BlenderJobPart jp = new BlenderJobPart(job.ID);
                        jp.TileX = x;
                        jp.TileY = y;
                        jp.Resolution = Resolution;
                        jp.Layers = Layers;
                        jp.FilePath = FilePath;
                    }
                }
            }

            return jobs.ToArray();
        }
    }
}
