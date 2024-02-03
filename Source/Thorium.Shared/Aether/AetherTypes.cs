using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.Aether
{
    public enum AetherTypeId : byte
    {
        Null = 0,
        Int8 = 1,
        Int16 = 2,
        Int32 = 3,
        Int64 = 4,
        UInt8 = 5,
        UInt16 = 6,
        UInt32 = 7,
        UInt64 = 8,
        Float = 9,
        Double = 10,
        True = 11,
        False = 12,
        String = 13,
        Bytes = 14,
        Array = 15,
        Bool = 16,
        AetherType = 255,
    }

    public interface IAetherType
    {
        public void WriteTo(AetherStream stream);
        public void ReadFrom(AetherStream stream);
    }

    public static class AetherTypes
    {
        public static readonly Dictionary<Type, AetherTypeId> typeToAetherType = new(){
            {null, AetherTypeId.Null},
            {typeof(sbyte), AetherTypeId.Int8 },
            {typeof(short), AetherTypeId.Int16 },
            {typeof(int), AetherTypeId.Int32 },
            {typeof(long), AetherTypeId.Int64 },
            {typeof(byte), AetherTypeId.UInt8 },
            {typeof(ushort), AetherTypeId.UInt16 },
            {typeof(uint), AetherTypeId.UInt32 },
            {typeof(ulong), AetherTypeId.UInt64 },
            {typeof(float), AetherTypeId.Float },
            {typeof(double), AetherTypeId.Double },
            {typeof(string), AetherTypeId.String },
            {typeof(byte[]), AetherTypeId.Bytes },
            {typeof(bool), AetherTypeId.Bool },
            };

        public static readonly Dictionary<AetherTypeId, Type> aetherTypeToType = new();

        static AetherTypes()
        {
            foreach (var kv in typeToAetherType)
            {
                aetherTypeToType[kv.Value] = kv.Key;
            }
        }
    }

}
