using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Thorium.Shared.Aether
{
    public class AetherStream : IDisposable
    {
        readonly DefaultSerializer defaultSerializer = new();
        public Dictionary<Type, IAetherSerializer> Serializers { get; } = [];

        public Stream BaseStream { get; }

        public readonly BinaryWriter writer;
        public readonly BinaryReader reader;

        public AetherStream(Stream stream, bool leaveOpen = true)
        {
            BaseStream = stream;

            if (stream.CanWrite)
            {
                writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen);
            }
            if (stream.CanRead)
            {
                reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen);
            }
        }

        private AetherTypeId GetAetherTypeId(Type type)
        {
            AetherTypeId id;
            if (type.IsArray)
            {
                id = AetherTypeId.Array;
            }
            else if (!AetherTypes.typeToAetherType.TryGetValue(type, out id))
            {
                return AetherTypeId.Object;
                //throw new NotSupportedException("Cant Serialize type of " + type.AssemblyQualifiedName);
            }
            return id;
        }

        private AetherTypeId GetAetherTypeId(object value)
        {
            var type = value.GetType();
            AetherTypeId id;
            if (value is bool _bool)
            {
                id = _bool ? AetherTypeId.True : AetherTypeId.False;
            }
            else if (type.IsArray)
            {
                id = AetherTypeId.Array;
            }
            else if (!AetherTypes.typeToAetherType.TryGetValue(type, out id))
            {
                return AetherTypeId.Object;
                //throw new NotSupportedException("Cant Serialize type of " + type.AssemblyQualifiedName);
            }
            return id;
        }

        public void Write(object value)
        {
            if (value == null)
            {
                writer.Write((byte)AetherTypeId.Null);
                return;
            }

            var type = value.GetType();
            AetherTypeId id = GetAetherTypeId(value);

            writer.Write((byte)id);

            switch (id)
            {
                case AetherTypeId.Int8:
                    writer.Write((sbyte)value);
                    break;
                case AetherTypeId.Int16:
                    writer.Write((short)value);
                    break;
                case AetherTypeId.Int32:
                    writer.Write((int)value);
                    break;
                case AetherTypeId.Int64:
                    writer.Write((long)value);
                    break;
                case AetherTypeId.UInt8:
                    writer.Write((byte)value);
                    break;
                case AetherTypeId.UInt16:
                    writer.Write((ushort)value);
                    break;
                case AetherTypeId.UInt32:
                    writer.Write((uint)value);
                    break;
                case AetherTypeId.UInt64:
                    writer.Write((ulong)value);
                    break;
                case AetherTypeId.Float:
                    writer.Write((float)value);
                    break;
                case AetherTypeId.Double:
                    writer.Write((double)value);
                    break;
                case AetherTypeId.String:
                    writer.Write((string)value);
                    break;
                case AetherTypeId.Bytes:
                    var bytes = (byte[])value;
                    writer.Write7BitEncodedInt(bytes.Length);
                    writer.Write(bytes);
                    break;
                case AetherTypeId.Array:
                    var array = (Array)value;
                    var elementType = type.GetElementType();
                    var elementTypeId = GetAetherTypeId(elementType);
                    writer.Write((byte)elementTypeId);
                    if (elementTypeId == AetherTypeId.Object)
                    {
                        writer.Write(elementType.AssemblyQualifiedName);
                        writer.Write7BitEncodedInt(array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                            WriteObject(array.GetValue(i),elementType); //TODO: has a lookup each time, get serializer here instead
                        }
                    }
                    else
                    {
                        writer.Write7BitEncodedInt(array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                            Write(array.GetValue(i));
                        }
                    }
                    break;
                case AetherTypeId.Object:
                    WriteObject(value);
                    break;
                case AetherTypeId.True:
                case AetherTypeId.False:
                    //value included in id
                    break;
                default:
                    throw new Exception("unhandled type " + value.GetType());
            }
        }

        private void WriteObject(object value)
        {
            Type type = value.GetType();
            writer.Write(type.AssemblyQualifiedName);
            WriteObject(value, type);
        }

        private void WriteObject(object value, Type type)
        {
            if (Serializers.TryGetValue(type, out IAetherSerializer serializer))
            {
                serializer.WriteTo(this, value);
            }
            else
            {
                defaultSerializer.WriteTo(this, value, type);
            }
        }

        public object Read()
        {
            AetherTypeId id = (AetherTypeId)reader.ReadByte();
            return Read(id);
        }

        private object ReadObject()
        {
            string assemblyQualifiedName = reader.ReadString();
            Type type = Type.GetType(assemblyQualifiedName);
            return ReadObject(type);
        }

        private object ReadObject(Type type)
        {
            if (Serializers.TryGetValue(type, out IAetherSerializer serializer))
            {
                return serializer.ReadFrom(this);
            }
            else
            {
                return defaultSerializer.ReadFrom(this, type);
            }
        }

        private object Read(AetherTypeId id)
        {
            switch (id)
            {
                case AetherTypeId.Null:
                    return null;
                case AetherTypeId.Int8:
                    return reader.ReadSByte();
                case AetherTypeId.Int16:
                    return reader.ReadInt16();
                case AetherTypeId.Int32:
                    return reader.ReadInt32();
                case AetherTypeId.Int64:
                    return reader.ReadInt64();
                case AetherTypeId.UInt8:
                    return reader.ReadByte();
                case AetherTypeId.UInt16:
                    return reader.ReadUInt16();
                case AetherTypeId.UInt32:
                    return reader.ReadUInt32();
                case AetherTypeId.UInt64:
                    return reader.ReadUInt64();
                case AetherTypeId.Float:
                    return reader.ReadSingle();
                case AetherTypeId.Double:
                    return reader.ReadDouble();
                case AetherTypeId.True:
                    return true;
                case AetherTypeId.False:
                    return false;
                case AetherTypeId.String:
                    return reader.ReadString();
                case AetherTypeId.Bytes:
                    int len = reader.Read7BitEncodedInt();
                    return reader.ReadBytes(len);
                case AetherTypeId.Array:
                    AetherTypeId elementTypeId = (AetherTypeId)reader.ReadByte();
                    if (elementTypeId == AetherTypeId.Object)
                    {
                        Type elementType = Type.GetType(reader.ReadString());
                        len = reader.Read7BitEncodedInt();
                        Array array = Array.CreateInstance(elementType, len);
                        for (int i = 0; i < len; i++)
                        {
                            array.SetValue(ReadObject(elementType), i);
                        }
                        return array;
                    }
                    else
                    {
                        Type elementType = AetherTypes.aetherTypeToType[elementTypeId];
                        len = reader.Read7BitEncodedInt();
                        Array array = Array.CreateInstance(elementType, len);
                        for (int i = 0; i < len; i++)
                        {
                            array.SetValue(Read(), i);
                        }
                        return array;
                    }
                case AetherTypeId.Object:
                    return ReadObject();
                default:
                    throw new NotSupportedException("unsupported aether type: " + id);
            }
        }


        public void Dispose()
        {
            writer.Dispose();
            reader.Dispose();
        }
    }
}
