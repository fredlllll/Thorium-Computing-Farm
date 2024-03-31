using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.Aether
{
    public class AetherSerializerLibrary
    {
        private readonly Dictionary<Type, IAetherSerializer> serializers = new();
        private List<AetherSerializerLibrary> libraries = new();

        public void Add(IAetherSerializer serializer)
        {
            serializers.Add(serializer.SerializedType, serializer);
        }

        public bool Remove(Type t)
        {
            return serializers.Remove(t);
        }

        public void Add(AetherSerializerLibrary library)
        {
            libraries.Add(library);
        }

        public bool Remove(AetherSerializerLibrary library)
        {
            return libraries.Remove(library);
        }

        public IAetherSerializer GetSerializer(Type t)
        {
            if (serializers.TryGetValue(t, out IAetherSerializer serializer))
            {
                return serializer;
            }
            foreach (var library in libraries)
            {
                serializer = library.GetSerializer(t);
                if (serializer != null)
                {
                    return serializer;
                }
            }
            return null;
        }

        public bool ContainsSerializerForType(Type t)
        {
            return GetSerializer(t) != null;
        }

        public AetherSerializerLibrary GetFlattened()
        {
            var flatLib = new AetherSerializerLibrary();
            foreach (var kv in serializers)
            {
                flatLib.Add(kv.Value);
            }
            foreach (var lib in libraries)
            {
                foreach (var kv in lib.serializers)
                {
                    flatLib.Add(kv.Value);
                }
            }
            return flatLib;
        }
    }
}