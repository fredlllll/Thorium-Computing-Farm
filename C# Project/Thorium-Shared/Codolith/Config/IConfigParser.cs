using System.Collections.Generic;
using System.IO;

namespace Codolith.Config
{
    public interface IConfigParser
    {
        Stream Stream { get; set; }
        void FillDictionary(Dictionary<string,string> dict);
    }
}
