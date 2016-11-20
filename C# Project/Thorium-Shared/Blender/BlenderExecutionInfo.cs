using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium_Shared.ExecutionActions;

namespace Thorium_Shared.Blender
{
    public class BlenderExecutionInfo : ITaskExecutionInfo
    {
        Config data;

        int frame;
        int tilesPerFrame;
        int tile;
        Layer[] layers;
        public byte[] zipData;
        string filename;
        Resolution resolution;

        FileInfo tmpZip;
        DirectoryInfo workingDir;

        public BlenderExecutionInfo(Config data)
        {
            this.data = data;

            frame = data.GetInt("frame");
            tilesPerFrame = data.GetInt("tilesPerFrame");
            tile = data.GetInt("tile");
            layers = data.GetString("layers").Split(',').Select((x)=> { return Layer.Parse(x); }).ToArray();
            filename = data.GetString("filename");
            resolution = Resolution.Parse(data.GetString("resolution"));
        }

        public void Setup()
        {
            tmpZip = new FileInfo(Path.GetRandomFileName());
            File.WriteAllBytes(tmpZip.FullName, zipData);

            workingDir = new DirectoryInfo(Path.GetRandomFileName());
            if(workingDir.Exists)
            {
                workingDir.Delete();
            }
            workingDir.Create();
            ZipFile.ExtractToDirectory(tmpZip.FullName,workingDir.FullName);
        }

        public void Run()
        {
            //run blender
            RunExecutableAction rea = new RunExecutableAction();
            rea.ExecutionFolder = workingDir.FullName;
            rea.ExecutableFile = "blender";
            rea.Arguments = new string[] { };//TODO
            rea.Execute();
        }

        public void Cleanup()
        {
            //workingDir.Delete();
        }

        public byte[] GetResultsZip()
        {
            throw new NotImplementedException();
        }
    }
}
