using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thorium_Shared
{
    public struct Resolution
    {
        public int width, height;

        public static Resolution Parse(string str)
        {
            Resolution r = new Resolution();

            string[] sa = str.Split('x');
            r.width = int.Parse(sa[0]);
            r.height = int.Parse(sa[1]);
            return r;
        }


        public override string ToString()
        {
            return width + "x" + height;
        }
    }
}
