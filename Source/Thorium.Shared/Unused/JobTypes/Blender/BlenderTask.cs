/*using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.Services;
using Thorium.Shared.Services.Server;
using Codolith.Config;

namespace Thorium.Shared.Blender
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
            frame = data.Get<int>("frame");
            tilesPerFrame = data.Get<int>("tilesPerFrame");
            tile = data.Get<int>("tile");
            layers = data.Get("layers").Split(',').Select((x) => { return Layer.Parse(x); }).ToArray();
            filename = data.Get("filename");
            resolution = Resolution.Parse(data.Get("resolution"));
            outputDirectory = new DirectoryInfo(data.Get("outputDirectory"));
        }

        public override ITaskExecutionInfo GetExecutionInfo()
        {
            BlenderExecutionInfo bei = new BlenderExecutionInfo(Data);
            return bei;
        }

        public override void FinalizeTask()
        {
            var cache = ServiceManager.Instance.GetService<IResultsCache>();
            //var cache = SharedData.Get<AServiceManager<IServerService>>(ServerConfigConstants.SharedDataID_ServerServiceManager).GetService<ResultsCache>();

            FileInfo resultFile = new FileInfo(Path.Combine(outputDirectory.FullName, Path.GetFileNameWithoutExtension(filename) + "_" + frame.ToString() + "_" + tile.ToString()));
            File.WriteAllBytes(resultFile.FullName, cache.GetResult(GetJobID() + GetID(), true));
        }
    }
}
*/