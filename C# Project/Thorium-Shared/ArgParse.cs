using System.Collections.Generic;

namespace Thorium_Shared
{
    public class ArgParse
    {
        public List<string> PositionalArgs { get; } = new List<string>();

        public Dictionary<string, string> ParsedArgs { get; } = new Dictionary<string, string>();

        public void Parse(string[] args)
        {
            ParsedArgs.Clear();

            string name = null;
            int unnamedIndex = 0;
            for(int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if(name == null)
                {
                    if(arg.StartsWith("--"))
                    {
                        name = arg.Substring(2);
                    }
                    else
                    {
                        name = PositionalArgs[unnamedIndex++];
                        ParsedArgs[name] = arg;
                        name = null;
                    }
                }
                else
                {
                    ParsedArgs[name] = arg;
                    name = null;
                }
            }
        }
    }
}
