using System;

namespace Thorium.Shared.Aether
{
    public interface IAetherSerializer
    {
        public Type SerializedType { get; }
        public void WriteTo(AetherStream stream, object value);
        public object ReadFrom(AetherStream stream);
    }

    /*public interface IAetherSerializer<T> :IAetherSerializer
    {
        public void WriteTo(AetherStream stream, T value);
        public T ReadFrom(AetherStream stream);
    }*/
}
