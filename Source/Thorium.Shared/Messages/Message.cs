using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Messages
{
    public abstract class Message : IAetherType
    {
        public abstract void WriteTo(AetherStream stream);
        public abstract void ReadFrom(AetherStream stream);
    }
}
