using System.Collections.Generic;

namespace Thorium.Shared.Database.Models
{
    public class Operation : Model
    {
        public required Job Job { get; set; }
        public required int OperationIndex { get; set; }
        public required string Type { get; set; }
        public required Dictionary<string, string> Data { get; set; }
    }
}
