using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Jobtypes.Blender
{
    public class BlenderExecutioner : AExecutioner
    {
        public BlenderExecutioner(LightweightTask t) : base(t)
        {
        }

        public override void Execute()
        {
            string blenderExecutable = Task.GetInfo<string>("blenderExecutable");
            string filename = Task.GetInfo<string>("filename");
            int frame = Task.GetInfo<int>("frame");

            string outputFile = Path.Combine(Directories.TempDir, Task.JobID, Task.ID, "frame" + frame + ".png");
            Console.WriteLine("outputFile: " + outputFile);

            RunExecutableAction rea = new RunExecutableAction
            {
                FileName = blenderExecutable
            };
            rea.AddArgument("-b"); //console mode "background"
            rea.AddArgument(filename);
            //rea.AddArgument("-y");//auto run python scripts
            rea.AddArgument("-o");//output file
            rea.AddArgument(outputFile);
            //rea.AddArgument("-F"); //format
            //rea.AddArgument("PNG"); //PNG
            //rea.AddArgument("-E"); //engine
            //rea.AddArgument("cycles"); //cycles
            //rea.AddArgument("-P"); //run this script on startup
            //rea.AddArgument("somescript.py");//the script
            //rea.AddArgument("-a");// use settings from blend file
            rea.AddArgument("-f");
            rea.AddArgument(frame.ToString());
            rea.AddArgument("--"); //blender stops parsing after this, but can be parsed by python scripts

            //TODO: add custom arguments

            rea.StartAndWait();
        }
    }
}
