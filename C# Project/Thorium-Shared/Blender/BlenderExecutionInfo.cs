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
        string[] layers;
        byte[] zipData;
        string filename;

        FileInfo tmpZip;
        DirectoryInfo workingDir;

        public BlenderExecutionInfo(Config data)
        {
            this.data = data;

            frame = data.GetInt("frame");
            tilesPerFrame = data.GetInt("tilesPerFrame");
            tile = data.GetInt("tile");
            layers = data.GetString("layers").Split(',');
            filename = data.GetString("filename");
        }

        public void Setup()
        {
            tmpZip = new FileInfo(Path.GetRandomFileName());
            using(FileStream fs = new FileStream(tmpZip.FullName, FileMode.Truncate))
            {
                fs.Write(zipData, 0, zipData.Length);
            }
            workingDir = new DirectoryInfo(Path.GetRandomFileName());
            if(workingDir.Exists)
            {
                workingDir.Delete();
            }
            workingDir.Create();
            ZipFile.ExtractToDirectory(tmpZip.FullName,workingDir.FullName);
            //ZipFile.ExtractToDirectory
            //TODO: unpack zip
        }

        public void Run()
        {
            //run blender
            RunExecutableAction rea = new RunExecutableAction();
            rea.ExecutionFolder = workingDir.FullName;
            rea.ExecutableFile = "blender";
            rea.Arguments = new string[] { };//TODO
            rea.Execute();
            //TODO: gotta get the result back to the main server
        }

        public byte[] GetResultZip()
        {
            throw new NotImplementedException();
        }
    }
}
