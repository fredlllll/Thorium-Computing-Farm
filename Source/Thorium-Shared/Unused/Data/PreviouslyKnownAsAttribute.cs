using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared.Data
{
    public class PreviouslyKnownAsAttribute : Attribute
    {
        public string Name { get; set; }

        public PreviouslyKnownAsAttribute(string name)
        {
            Name = name;
        }
    }
}
