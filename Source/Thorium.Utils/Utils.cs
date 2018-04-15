using System;
using System.Linq;

namespace Thorium_Utils
{
    public static class Utils
    {
        public static Random R { get; } = new Random();

        public static string GetRandomGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[R.Next(s.Length)]).ToArray());
        }
    }
}
