using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;
using Thorium_Shared.Services.Server;

namespace Thorium_Shared.Blender
{
    public class BlenderTask : Task
    {
        int frame;
        int tilesPerFrame;
        int tile;
        Layer[] layers;
        string filename;
        DirectoryInfo outputDirectory;
        Resolution resolution;

        public BlenderTask(string parentJobID, Config data) : base(parentJobID, data)
        {
            frame = data.GetInt("frame");
            tilesPerFrame = data.GetInt("tilesPerFrame");
            tile = data.GetInt("tile");
            layers = data.GetString("layers").Split(',').Select((x) => { return Layer.Parse(x); }).ToArray();
            filename = data.GetString("filename");
            resolution = Resolution.Parse(data.GetString("resolution"));
            outputDirectory = new DirectoryInfo(data.GetString("outputDirectory"));
        }

        public override ITaskExecutionInfo GetExecutionInfo()
        {
            BlenderExecutionInfo bei = new BlenderExecutionInfo(Data);
            return bei;
        }

        public override void FinalizeTask()
        {
            var cache = SharedData.Get<AServiceManager<IServerService>>(ServerConfigConstants.SharedDataID_ServerServiceManager).GetService<ResultsCache>();

            FileInfo resultFile = new FileInfo(Path.Combine(outputDirectory.FullName, Path.GetFileNameWithoutExtension(filename) + "_" + frame.ToString() + "_" + tile.ToString()));
            File.WriteAllBytes(resultFile.FullName, cache.GetResult(GetJobID() + GetID(), true));
        }
    }
}
