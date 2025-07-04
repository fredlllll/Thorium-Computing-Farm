using System;

namespace Thorium.Shared.Database.Models
{
    public class Model
    {
        public required string Id { get; set; }
        public required DateTime Created { get; set; }
        public required DateTime Updated { get; set; }
    }
}
