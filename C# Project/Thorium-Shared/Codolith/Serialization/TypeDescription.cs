namespace Codolith.Serialization
{
    public class TypeDescription
    {
        public TypeDescription(string typeName)
        {
            TypeName = typeName;
        }

        public string TypeName { get; private set; }
    }
}