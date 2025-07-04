using System.Collections.Generic;

namespace Thorium.Shared.DTOs
{
    public class OperationDTO
    {
        public required string OperationType { get; set; }
        public required Dictionary<string,string> OperationData { get; set; }
    }
}
