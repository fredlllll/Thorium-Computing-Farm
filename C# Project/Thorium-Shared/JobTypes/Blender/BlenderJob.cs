using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.Services;
using Thorium_Shared.Services.Server;

namespace Thorium_Shared.Blender
{
    public class BlenderJob : Job
    {
        List<FrameBounds> frameBounds = new List<FrameBounds>();
        int tilesPerFrame;
        List<Layer> layers = new List<Layer>();
        string filename;
        Resolution resolution;
        BlenderRenderEngine engine = BlenderRenderEngine.Cycles;
        BlenderEngineConfig engineConfig = new BlenderEngineConfig();
        DirectoryInfo jobOutputDirectory;

        public BlenderJob(Config data) : base(data)
        {
            DataPackageProviderServer packageProvider = ServiceManager.Instance.GetService<DataPackageProviderServer>();

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
            packageProvider.RegisterPackage(ID, di);
            /*DirectoryInfo tmpDir = new DirectoryInfo(SharedData.Get<Config>(ServerConfigConstants.SharedDataID_ServerConfig).GetString(ServerConfigConstants.tmpFolder));
            zipFile = new FileInfo(Path.Combine(tmpDir.FullName, ID + "_data.zip"));
            if(zipFile.Exists)
            {
                zipFile.Delete();
            }
            ZipFile.CreateFromDirectory(dir, zipFile.FullName);*/
            filename = data.GetString("filename");
            resolution = Resolution.Parse(data.GetString("resolution"));
            engine = (BlenderRenderEngine)Enum.Parse(typeof(BlenderRenderEngine), data.GetString("engine"));
            engineConfig = BlenderEngineConfig.Create(data);
            jobOutputDirectory = new DirectoryInfo(data.GetString("outputDirectory"));
            Directory.CreateDirectory(jobOutputDirectory.FullName);
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
                        c.Set("jobID", ID);
                        c.Set("frame", f);
                        c.Set("tilesPerFrame", tilesPerFrame);
                        c.Set("tile", t);
                        c.Set("layers", layersString);
                        c.Set("filename", filename);
                        c.Set("resolution", resolution);
                        c.Set("outputDirectory", jobOutputDirectory.FullName);
                        var bt = new BlenderTask(ID, c);
                        c.Set("taskID", bt.GetID());
                        tasks.Add(bt);
                    }
                }
            }
        }

        public override void OnJobFinished()
        {
            //TODO: unregister package i guess
        }
    }
}
