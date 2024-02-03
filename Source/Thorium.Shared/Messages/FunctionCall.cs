using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Messages
{
    public class FunctionCall : Message
    {
        public int Id { get; set; }
        public string FunctionName { get; set; }
        public object[] FunctionArguments { get; set; }
        public bool NeedsAnwer { get; set; }

        public override void ReadFrom(AetherStream stream)
        {
            Id = stream.reader.ReadInt32();
            FunctionName = stream.reader.ReadString();

            int len = stream.reader.Read7BitEncodedInt();
            FunctionArguments = new object[len];
            for (int i = 0; i < len; i++)
            {
                FunctionArguments[i] = stream.Read();
            }

            NeedsAnwer = stream.reader.ReadBoolean();
        }

        public override void WriteTo(AetherStream stream)
        {
            stream.writer.Write(Id);
            stream.writer.Write(FunctionName);

            stream.writer.Write7BitEncodedInt(FunctionArguments.Length);
            for (int i = 0; i < FunctionArguments.Length; i++)
            {
                stream.Write(FunctionArguments[i]);
            }

            stream.writer.Write(NeedsAnwer);
        }
    }
}
