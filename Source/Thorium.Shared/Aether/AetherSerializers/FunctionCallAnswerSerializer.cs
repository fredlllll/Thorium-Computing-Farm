using System;
using Thorium.Shared.Messages;

namespace Thorium.Shared.Aether.AetherSerializers
{
    public class FunctionCallAnswerSerializer : IAetherSerializer
    {
        public Type SerializedType => typeof(FunctionCallAnswer);
        public object ReadFrom(AetherStream stream)
        {
            var answer = new FunctionCallAnswer();

            answer.Id = stream.reader.ReadInt32();
            answer.ReturnValue = stream.Read();
            answer.Exception = (string)stream.Read();

            return answer;
        }

        public void WriteTo(AetherStream stream, object value)
        {
            var answer = (FunctionCallAnswer)value;
            stream.writer.Write(answer.Id);
            stream.Write(answer.ReturnValue);
            stream.Write(answer.Exception);
        }
    }
}
