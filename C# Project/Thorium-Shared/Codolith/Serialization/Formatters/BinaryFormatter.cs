using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization.Formatters
{
    public class BinaryFormatter : IFormatter
    {
        static MultiDictionary<Type, byte, Action<BinaryWriter, object>, Func<BinaryReader, object>> primitiveTypes = new MultiDictionary<Type, byte, Action<BinaryWriter, object>, Func<BinaryReader, object>>();

        static BinaryFormatter()
        {
            byte index = 0;
            foreach(var t in Utils.PrimitiveTypes)
            {
                var mt = new ModTuple<Type, byte, Action<BinaryWriter, object>, Func<BinaryReader, object>>();
                mt.Value1 = t;
                mt.Value2 = index++;
                mt.Value3 = valueWriters[t];
                mt.Value4 = valueReaders[t];
                primitiveTypes[t] = mt;
            }
        }

        static Dictionary<Type, Action<BinaryWriter, object>> valueWriters = new Dictionary<Type, Action<BinaryWriter, object>>()
        {
            {typeof(sbyte),(bw,x)=> {bw.Write((sbyte)x); } },
            {typeof(byte),(bw,x)=> {bw.Write((byte)x); } },
            {typeof(short),(bw,x)=> {bw.Write((short)x); } },
            {typeof(ushort),(bw,x)=> {bw.Write((ushort)x); } },
            {typeof(int),(bw,x)=> {bw.Write((int)x); } },
            {typeof(uint),(bw,x)=> {bw.Write((uint)x); } },
            {typeof(long),(bw,x)=> {bw.Write((long)x); } },
            {typeof(ulong),(bw,x)=> {bw.Write((ulong)x); } },
            {typeof(float),(bw,x)=> {bw.Write((float)x); } },
            {typeof(double),(bw,x)=> {bw.Write((double)x); } },
            {typeof(string),(bw,x)=> {bw.Write((string)x); } },
            {typeof(bool),(bw,x)=> {bw.Write((bool)x); } },
            {typeof(char),(bw,x)=> {bw.Write((char)x); } },

            {typeof(decimal),(bw,x)=> {bw.Write((decimal)x); } },
        };

        static Dictionary<Type, Func<BinaryReader, object>> valueReaders = new Dictionary<Type, Func<BinaryReader, object>>()
        {
            {typeof(sbyte),(br)=> {return br.ReadSByte(); } },
            {typeof(byte),(br)=> {return br.ReadByte(); } },
            {typeof(short),(br)=> {return br.ReadInt16(); } },
            {typeof(ushort),(br)=> {return br.ReadUInt16(); } },
            {typeof(int),(br)=> {return br.ReadInt32(); } },
            {typeof(uint),(br)=> {return br.ReadUInt32(); } },
            {typeof(long),(br)=> {return br.ReadInt64(); } },
            {typeof(ulong),(br)=> {return br.ReadUInt64(); } },
            {typeof(float),(br)=> {return br.ReadSingle(); } },
            {typeof(double),(br)=> {return br.ReadDouble(); } },
            {typeof(string),(br)=> {return br.ReadString(); } },
            {typeof(bool),(br)=> {return br.ReadBoolean(); } },
            {typeof(char),(br)=> {return br.ReadChar(); } },

            {typeof(decimal),(br)=> {return br.ReadDecimal(); } },
        };

        BinaryWriter bw;
        BinaryReader br;

        public BinaryFormatter(Stream stream)
        {
            bw = new BinaryWriter(stream);
            br = new BinaryReader(stream);
        }

        public void Write(SerializationDataSet dataSet)
        {
            bw.Write(dataSet.typeDescriptions.Count);
            foreach(var td in dataSet.typeDescriptions)
            {
                bw.Write(td.TypeName);
            }

            bw.Write(dataSet.objectDataSets.Count);
            foreach(var ods in dataSet.objectDataSets)
            {
                bw.Write(ods.TypeIndex);

                bw.Write(ods.primitives.Count);
                foreach(var p in ods.primitives)
                {
                    WritePrimitive(p);
                }
                bw.Write(ods.complexPrimitives.Count);
                foreach(var p in ods.complexPrimitives)
                {
                    WritePrimitive(p);
                }
            }
        }

        private void WritePrimitive(Primitive p)
        {
            var mt = primitiveTypes[p.Value.GetType()];
            bw.Write((byte)mt.Value2);
            bw.Write(p.Name);
            mt.Value3(bw, p.Value);
        }

        public SerializationDataSet Read()
        {
            SerializationDataSet sds = new SerializationDataSet();

            int count = br.ReadInt32();
            for(int i = 0; i < count; i++)
            {
                sds.typeDescriptions.Add(new TypeDescription(br.ReadString()));
            }

            count = br.ReadInt32();
            for(int i = 0; i < count; i++)
            {
                var ods = new ObjectSerializationDataSet();
                ods.TypeIndex = br.ReadInt32();

                int count2 = br.ReadInt32();
                for(int j = 0; j < count2; j++)
                {
                    ods.primitives.Add(ReadPrimitive());
                }

                count2 = br.ReadInt32();
                for(int j = 0; j < count2; j++)
                {
                    ods.complexPrimitives.Add(ReadPrimitive());
                }
                sds.objectDataSets.Add(ods);
            }

            return sds;
        }

        private Primitive ReadPrimitive()
        {
            Primitive p = new Primitive();
            byte typeID = br.ReadByte();
            p.Name = br.ReadString();
            var mt = primitiveTypes[typeID];
            p.Value = mt.Value4(br);

            return p;
        }
    }
}
