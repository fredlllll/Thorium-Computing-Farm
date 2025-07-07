using System.Collections.Generic;

namespace Thorium.Shared.Database.Models
{
    public class Node :Model
    {
        public required string Identifier { get; set; }
        public string? CurrentTaskId { get; set; }
    }
}
