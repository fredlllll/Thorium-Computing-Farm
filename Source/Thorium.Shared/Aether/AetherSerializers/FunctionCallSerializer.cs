using Thorium.Shared.Messages;

namespace Thorium.Shared.Aether.AetherSerializers
{
    public class FunctionCallSerializer : IAetherSerializer
    {
        public object ReadFrom(AetherStream stream)
        {
            var call = new FunctionCall();

            call.Id = stream.reader.ReadInt32();
            call.FunctionName = stream.reader.ReadString();

            int len = stream.reader.Read7BitEncodedInt();
            call.FunctionArguments = new object[len];
            for (int i = 0; i < len; i++)
            {
                call.FunctionArguments[i] = stream.Read();
            }

            call.NeedsAnwer = stream.reader.ReadBoolean();
            return call;
        }

        public void WriteTo(AetherStream stream, object value)
        {
            var call = (FunctionCall)value;
            stream.writer.Write(call.Id);
            stream.writer.Write(call.FunctionName);

            stream.writer.Write7BitEncodedInt(call.FunctionArguments.Length);
            for (int i = 0; i < call.FunctionArguments.Length; i++)
            {
                stream.Write(call.FunctionArguments[i]);
            }

            stream.writer.Write(call.NeedsAnwer);
        }
    }
}
