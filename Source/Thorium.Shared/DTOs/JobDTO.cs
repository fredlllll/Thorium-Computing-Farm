using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.DTOs
{
    public class JobDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int TaskCount { get; set; }

        public OperationDTO[] Operations { get; set; }
        //TODO: all the other things
    }
}
