using System.Collections.Generic;
using System.IO;

namespace Codolith.Config
{
    public interface IConfigSaver
    {
        Stream Stream { get; set; }
        void SaveDictionary(Dictionary<string,string> dict);
    }
}
