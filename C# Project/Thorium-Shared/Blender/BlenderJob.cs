using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Blender
{
    public class BlenderJob : Job
    {
        List<FrameBounds> frameBounds = new List<FrameBounds>();
        int tilesPerFrame;
        List<Layer> layers = new List<Layer>();
        FileInfo zipFile;
        string filename;
        BlenderRenderEngine engine = BlenderRenderEngine.Cycles;
        BlenderEngineConfig engineConfig = new BlenderEngineConfig();

        public BlenderJob(Config data) : base(data)
        {
            var fbs = data.GetString("frameBounds").Split(',');
            foreach(var s in fbs)
            {
                frameBounds.Add(FrameBounds.Parse(s));
            }
            tilesPerFrame = data.GetInt("tilesPerFrame");
            var ls = data.GetString("layers").Split(',');
            foreach(var s in ls)
            {
                layers.Add(Layer.Parse(s));
            }
            var dir = data.GetString("sourceDirectory");
            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo tmpDir = new DirectoryInfo(SharedData.Get<Config>("serverConfig").GetString("tmpFolder"));
            zipFile = new FileInfo(Path.Combine(tmpDir.FullName, ID + "_data.zip"));
            if(zipFile.Exists)
            {
                zipFile.Delete();
            }
            ZipFile.CreateFromDirectory(dir, zipFile.FullName);
            filename = data.GetString("filename");
            engine = (BlenderRenderEngine)Enum.Parse(typeof(BlenderRenderEngine), data.GetString("engine"));
            engineConfig = BlenderEngineConfig.Create(data);
        }

        public override void InitializeTasks()
        {
            string layersString = string.Join(",", layers);
            foreach(var fb in frameBounds)
            {
                foreach(int f in fb.GetFrames())
                {
                    for(int t = 0; t < tilesPerFrame; t++)
                    {
                        Config c = new Config();
                        c.Set("frame", f);
                        c.Set("tilesPerFrame", tilesPerFrame);
                        c.Set("tile", t);
                        c.Set("layers", layersString);
                        c.Set("zipFile", zipFile.FullName);
                        c.Set("filename", filename);
                        var bt = new BlenderTask(ID, c);
                        tasks.Add(bt);
                    }
                }
            }
        }
    }
}
