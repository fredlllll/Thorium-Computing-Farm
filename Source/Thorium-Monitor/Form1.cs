using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Thorium_Shared;
using Thorium_Shared.Net;
using Thorium_Shared.Net.ServicePoint;

namespace Thorium_Monitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnStartJob_Click(object sender, System.EventArgs e)
        {
            string dataPackage = Utils.GetRandomID();
            string tmpDir = Path.Combine(Directories.TempDir, "datapackage");
            Directory.CreateDirectory(tmpDir);
            File.Copy(txtDataPackagePath.Text, Path.Combine(tmpDir, Path.GetFileName(txtDataPackagePath.Text)), true);
            Thorium_Storage_Service.StorageService.CreateDataPackage(dataPackage, tmpDir, true);

            JObject info = new JObject
            {
                [JobProperties.TaskProducerType] = "Thorium_Blender.BlenderTaskProducer",//typeof(BlenderTaskProducer).AssemblyQualifiedName,
                [JobAndTaskProperties.ExecutionerType] = "Thorium_Blender.BlenderExecutioner",//typeof(BlenderExecutioner).AssemblyQualifiedName,
                ["fileName"] = txtBlendFileName.Text,
                ["startFrame"] = (long)numStartFrame.Value,
                ["endFrame"] = (long)numEndFrame.Value,
                ["framesPerTask"] = (long)numFramesPerTask.Value,
                ["dataPackage"] = dataPackage,
                ["uploadType"] = "Async",
            };

            JObject arg = new JObject
            {
                ["jobName"] = txtJobName.Text,
                ["jobInformation"] = info
            };

            var client = new TCPServiceInvoker(txtServerHost.Text, (ushort)numServerPort.Value);
            JObject answer = client.Invoke(ServerControlCommands.AddJob, arg) as JObject;
            MessageBox.Show("The jobs id is: " + answer.Get<string>("id"));
        }

        private void BtnSearchDataPackage_Click(object sender, System.EventArgs e)
        {
            string oldFilter = ofd.Filter;

            ofd.Filter = "datapackage.zip|datapackage.zip";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                txtDataPackagePath.Text = ofd.FileName;
                using(FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Read);
                    foreach(var entry in zip.Entries)
                    {
                        if(entry.FullName.EndsWith(".blend"))
                        {
                            txtBlendFileName.Text = entry.Name;
                            break;
                        }
                    }
                }
            }
            ofd.Filter = oldFilter;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }
    }
}
