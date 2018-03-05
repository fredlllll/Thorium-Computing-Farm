using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NLog;

namespace Thorium_Processes
{
    public class LoggedProcessInfo : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        Process p;
        public Process Process
        {
            get { return p; }
        }
        string logPath;

        StreamWriter logWriter;

        Thread runThread;
        Thread outThread, errThread;

        ManualResetEvent waitHandle = new ManualResetEvent(false);
        public ManualResetEvent WaitHandle
        {
            get { return waitHandle; }
        }

        public event Action<LoggedProcessInfo> OnProcessExited;

        public LoggedProcessInfo(Process unstartedProcess, string logPath)
        {
            this.p = unstartedProcess;
            this.logPath = logPath;
        }

        public void Start()
        {
            logWriter = new StreamWriter(new FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.Read));

            runThread = new Thread(Run);
            outThread = new Thread(RunOut);
            errThread = new Thread(RunErr);
            p.Start();
            outThread.Start();
            errThread.Start();
            runThread.Start();
        }

        public void Stop()
        {
            p.Kill();
            waitHandle.Set();
        }

        private void Run()
        {
            outThread.Join();
            errThread.Join();
            logWriter.Dispose();
            p.WaitForExit();
            OnProcessExited?.Invoke(this);
            waitHandle.Set();
        }

        void RunOut()
        {
            ReceiveOnStream(p.StandardOutput);
        }

        void RunErr()
        {
            ReceiveOnStream(p.StandardError);
        }

        void ReceiveOnStream(StreamReader sr)
        {
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                ReceiveData(line);
            }
        }

        private void ReceiveData(string data)
        {
            lock(logWriter)
            {
                logger.Info(data);
                try
                {
                    logWriter.WriteLine(data);
                }
                catch(ObjectDisposedException)
                {
                    logger.Warn("couldnt write message to log cause it was already closed");
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    OnProcessExited = null;
                    waitHandle.Dispose();
                    logWriter.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
