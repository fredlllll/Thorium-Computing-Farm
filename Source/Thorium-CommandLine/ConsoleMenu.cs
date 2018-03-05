using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium_CommandLine
{
    public class ConsoleMenu
    {
        Dictionary<string, Action<string[]>> methods = new Dictionary<string, Action<string[]>>();

        public void AddMethod(string name, Action<string[]> callback)
        {
            if(name == "exitLoop")
            {
                throw new ArgumentException("the name exitLoop is reserved");
            }
            methods[name] = callback;
        }

        public void Stop()
        {
            running = false;
        }

        bool running = true;
        public void Run()
        {
            running = true;
            while(running)
            {
                string line = Console.ReadLine().Trim();
                if(line.Length == 0)
                {
                    continue;
                }
                List<string> parts = new List<string>();
                StringBuilder currentPart = new StringBuilder();
                bool inString = false;
                for(int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    if(inString)
                    {
                        if(c == '"')
                        {
                            inString = false;
                        }
                        else
                        {
                            currentPart.Append(c);
                        }
                    }
                    else
                    {
                        if(c == ' ')
                        {
                            parts.Add(currentPart.ToString());
                            currentPart.Clear();
                        }
                        else if(c == '"')
                        {
                            inString = true;
                        }
                        else
                        {
                            currentPart.Append(c);
                        }
                    }
                }
                if(currentPart.Length > 0)
                {
                    parts.Add(currentPart.ToString());
                }
                if(parts[0] == "exitLoop")
                {
                    break;
                }
                if(methods.TryGetValue(parts[0], out Action<string[]> method))
                {
                    parts.RemoveAt(0);
                    method(parts.ToArray());
                }
                else
                {
                    Console.WriteLine("Method " + parts[0] + " not found");
                }
                Console.WriteLine();
            }
        }
    }
}
