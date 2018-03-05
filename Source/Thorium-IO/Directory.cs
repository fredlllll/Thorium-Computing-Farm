using System;
using System.Collections.Generic;
using System.IO;

namespace Thorium_IO
{
    public static class Directory
    {
        public static void CopyDirectory(string source, string target)
        {
            var stack = new Stack<Tuple<string, string>>();
            stack.Push(new Tuple<string, string>(source, target));

            while(stack.Count > 0)
            {
                var folders = stack.Pop();
                System.IO.Directory.CreateDirectory(folders.Item2);
                foreach(var file in System.IO.Directory.GetFiles(folders.Item1, "*.*", SearchOption.TopDirectoryOnly))
                {
                    File.Copy(file, Path.Combine(folders.Item2, Path.GetFileName(file)));
                }

                foreach(var folder in System.IO.Directory.GetDirectories(folders.Item1))
                {
                    stack.Push(new Tuple<string, string>(folder, Path.Combine(folders.Item2, Path.GetFileName(folder))));
                }
            }
        }
    }
}
