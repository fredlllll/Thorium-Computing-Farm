using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Thorium.Shared.DTOs
{
    public class OperationDTO
    {
        public string OperationType {  get; set; }
        public JsonDocument OperationData { get; set; }
    }
}
