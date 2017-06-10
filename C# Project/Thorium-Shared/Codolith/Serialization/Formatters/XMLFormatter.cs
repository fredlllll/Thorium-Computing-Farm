using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Codolith.Serialization.Formatters
{
    public class XMLFormatter : IFormatter
    {
        static BidirectionalDict<Type, string> primitiveTypeIDs = new BidirectionalDict<Type, string>();

        static XMLFormatter()
        {
            foreach(Type t in Utils.PrimitiveTypes)
            {
                primitiveTypeIDs[t] = t.Name;
            }
        }

        Stream stream;

        XmlWriter wr;
        XmlDocument rd;

        public bool Indent { get; set; } = true;

        public XMLFormatter(Stream stream)
        {
            this.stream = stream;
        }

        public void Write(SerializationDataSet dataSet)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = Indent;
            wr = XmlWriter.Create(stream, settings);

            wr.WriteStartDocument();
            wr.WriteStartElement("data");

            wr.WriteStartElement("typedescriptions");
            foreach(var tds in dataSet.typeDescriptions)
            {
                wr.WriteStartElement("typedescription");
                wr.WriteValue(tds.TypeName);
                wr.WriteEndElement();
            }
            wr.WriteEndElement();

            wr.WriteStartElement("objectdatasets");
            foreach(var ods in dataSet.objectDataSets)
            {
                wr.WriteStartElement("objectdataset");

                wr.WriteStartElement("typeindex");
                wr.WriteValue(ods.TypeIndex);
                wr.WriteEndElement();

                wr.WriteStartElement("primitives");
                foreach(var p in ods.primitives)
                {
                    WritePrimitive(p);
                }
                wr.WriteEndElement();

                wr.WriteStartElement("complexprimitives");
                foreach(var p in ods.complexPrimitives)
                {
                    WritePrimitive(p);
                }
                wr.WriteEndElement();

                wr.WriteEndElement();
            }
            wr.WriteEndElement();
            wr.WriteEndDocument();

            wr.Flush();
        }

        private void WritePrimitive(Primitive p)
        {
            wr.WriteStartElement("primitive");
            wr.WriteAttributeString("type", primitiveTypeIDs[p.Value.GetType()].ToString(System.Globalization.CultureInfo.InvariantCulture));
            wr.WriteAttributeString("name", p.Name);
            wr.WriteValue(p.Value);
            wr.WriteEndElement();
        }

        public SerializationDataSet Read()
        {
            rd = new XmlDocument();
            rd.Load(stream);

            SerializationDataSet sds = new SerializationDataSet();

            var typeDescriptions = rd.SelectNodes("/data/typedescriptions/typedescription");
            foreach(XmlNode tds in typeDescriptions)
            {
                sds.typeDescriptions.Add(new TypeDescription(tds.InnerText));
            }

            var objDataSets = rd.SelectNodes("/data/objectdatasets/objectdataset");
            foreach(XmlNode ods in objDataSets)
            {
                ObjectSerializationDataSet osds = new ObjectSerializationDataSet();
                osds.TypeIndex = Convert.ToInt32(ods.SelectSingleNode("typeindex").InnerText, System.Globalization.CultureInfo.InvariantCulture);

                var primitives = ods.SelectNodes("primitives/primitive");
                foreach(XmlNode prim in primitives)
                {
                    osds.primitives.Add(ReadPrimitive(prim));
                }

                primitives = ods.SelectNodes("complexprimitives/primitive");
                foreach(XmlNode prim in primitives)
                {
                    osds.complexPrimitives.Add(ReadPrimitive(prim));
                }

                sds.objectDataSets.Add(osds);
            }

            return sds;
        }

        private Primitive ReadPrimitive(XmlNode node)
        {
            Primitive prim = new Primitive();
            string type = node.Attributes["type"].Value;
            prim.Name = node.Attributes["name"].Value;

            prim.Value = Convert.ChangeType(node.InnerText, primitiveTypeIDs[type], System.Globalization.CultureInfo.InvariantCulture);
            return prim;
        }
    }
}
