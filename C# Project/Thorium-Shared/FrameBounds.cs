using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public struct FrameBounds
    {
        public int frameStart, frameEnd;

        public IEnumerable<int> GetFrames()
        {
            return Enumerable.Range(frameStart, frameEnd - frameStart + 1);
        }

        public static FrameBounds Parse(string str)
        {
            FrameBounds fb = new FrameBounds();
            if(str.Contains('-'))
            {
                var sa = str.Split('-');
                fb.frameStart = int.Parse(sa[0]);
                fb.frameEnd = int.Parse(sa[1]);
            }
            else
            {
                fb.frameStart = fb.frameEnd = int.Parse(str);
            }
            return fb;
        }
    }
}
