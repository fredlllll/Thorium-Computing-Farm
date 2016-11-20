using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Blender
{
    public class BlenderTask : Task
    {
        int frame;
        int tilesPerFrame;
        int tile;
        Layer[] layers;
        FileInfo zipFile;
        string filename;
        Resolution resolution;

        public BlenderTask(string parentJobID, Config data) : base(parentJobID, data)
        {
            frame = data.GetInt("frame");
            tilesPerFrame = data.GetInt("tilesPerFrame");
            tile = data.GetInt("tile");
            layers = data.GetString("layers").Split(',').Select((x) => { return Layer.Parse(x); }).ToArray();
            zipFile = new FileInfo(data.GetString("zipFile"));
            filename = data.GetString("filename");
            resolution = Resolution.Parse(data.GetString("resolution"));
        }

        public override ITaskExecutionInfo GetExecutionInfo()
        {
            BlenderExecutionInfo bei = new BlenderExecutionInfo(Data);
            bei.zipData = File.ReadAllBytes(zipFile.FullName);
            return bei;
        }
    }
}
