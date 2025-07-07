using System;
using System.Collections.Generic;

namespace Thorium.Shared.Database.Models
{
    public class Job : Model
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
