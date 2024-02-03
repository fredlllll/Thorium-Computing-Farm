using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Messages
{
    public class FunctionCallAnswer : Message
    {
        public int Id { get; set; }
        public object ReturnValue { get; set; }
        public string Exception { get; set; }

        public override void ReadFrom(AetherStream stream)
        {
            Id = stream.reader.ReadInt32();
            ReturnValue = stream.Read();
            Exception = stream.reader.ReadString();
        }

        public override void WriteTo(AetherStream stream)
        {
            stream.writer.Write(Id);
            stream.Write(ReturnValue);
            stream.writer.Write(Exception);
        }
    }
}
