using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SO_CC2.Utils
{
    public static class BaseHelper
    {
        public static string ToBase(BigInteger value, string baseChars)
        {
            List<char> result = new();

            do
            {
                result.Add(baseChars[(int)(value % baseChars.Length)]);
                value = value / baseChars.Length;
            }
            while (value > 0);

            result.Reverse();

            return new string(result.ToArray());
        }

        public static BigInteger FromBase(string value, string baseChars)
        {
            BigInteger result = BigInteger.Zero;

            for (int i = 0; i < value.Length; i++)
            {
                int idx = value.Length - i - 1;
                result += baseChars.IndexOf(value[idx]) * BigInteger.Pow(baseChars.Length, i);
            }

            return result;
        }
    }
}
