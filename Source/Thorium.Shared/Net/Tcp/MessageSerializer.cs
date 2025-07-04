using System;
using Thorium.Shared.Aether;

namespace Thorium.Shared.Net.Tcp
{
    public class MessageSerializer : IAetherSerializer
    {
        public Type SerializedType => typeof(Message);
        public object ReadFrom(AetherStream stream)
        {
            var msg = new Message();
            msg.Id = stream.reader.ReadInt64();
            msg.Payload = stream.Read();
            return msg;
        }

        public void WriteTo(AetherStream stream, object value)
        {
            var msg = (Message)value;
            stream.writer.Write(msg.Id);
            stream.Write(msg.Payload);
        }
    }
}
