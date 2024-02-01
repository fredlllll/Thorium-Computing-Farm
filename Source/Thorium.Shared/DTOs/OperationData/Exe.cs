using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.DTOs.OperationData
{
    public class Exe
    {
        public string FilePath {  get; set; }
        public string[] Arguments { get; set; }
        public string WorkingDir {  get; set; }
    }
}
