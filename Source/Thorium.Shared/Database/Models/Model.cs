using System;

namespace Thorium.Shared.Database.Models
{
    public class Model
    {
        public required string Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
