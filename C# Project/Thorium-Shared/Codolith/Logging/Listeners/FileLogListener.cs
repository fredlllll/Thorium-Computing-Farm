using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Codolith.Logging.Listeners
{
    public class FileLogListener : ILogListener
    {
        StreamWriter sw = null;

        public FileInfo File { get; protected set; }
        public string HashtagMarker { get; set; } = "#####";

        public FileLogListener(string filename = default(string))
        {
            if(filename == default(string))
            {
                filename = Assembly.GetEntryAssembly().GetName().Name + ".log";
            }
            File = new FileInfo(filename);

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            PrintEndAndClose();
        }

        public void Log(string logmessage)
        {
            if(sw == null)
            {
                OpenStreamAndPrintStart();
            }
            sw.WriteLine(logmessage);
            sw.Flush();
        }

        private void OpenStreamAndPrintStart()
        {
            if(sw == null)
            {
                FileStream fs = File.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                fs.Seek(0, SeekOrigin.End);
                //add byte order mark only if at beginning of file
                sw = new StreamWriter(fs, new UTF8Encoding(fs.Position == 0));
                sw.WriteLine(HashtagMarker + " Start " + HashtagMarker);
            }
        }

        private void PrintEndAndClose()
        {
            if(sw != null)
            {
                sw.WriteLine(HashtagMarker + " End " + HashtagMarker);
                sw.Close();
                sw.Dispose();
                sw = null;
            }
        }

        public void Attach(Logger logger)
        {
        }

        public void Detach(Logger logger)
        {
            PrintEndAndClose();
        }
    }
}
