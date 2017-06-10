using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codolith.Serialization.Formatters
{
    public interface IFormatter
    {
        void Write(SerializationDataSet dataSet);
        SerializationDataSet Read();
    }
}
