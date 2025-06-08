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
        /// <summary>
        /// Rewrites a <paramref name="value"/> into any base, using the characters provided by <paramref name="baseChars"/>.
        /// </summary>
        /// <param name="value">The value to rewrite</param>
        /// <param name="baseChars">The character set of the base to write in</param>
        /// <param name="minDigits">The minimum amount of digits the rewritten value needs to have</param>
        /// <returns><paramref name="value"/>, rewritten in base <paramref name="baseChars"/>.Length, using the characters from <paramref name="baseChars"/></returns>
        public static string ToBase(BigInteger value, string baseChars, int minDigits = 0)
        {
            List<char> result = new();

            do
            {
                result.Add(baseChars[(int)(value % baseChars.Length)]);
                value = value / baseChars.Length;
            }
            while (value > 0);

            while (result.Count < minDigits)
                result.Add(baseChars[0]);

            result.Reverse();

            return new string(result.ToArray());
        }

        /// <summary>
        /// Computes the numerical value of <paramref name="value"/> written in the base provided by <paramref name="baseChars"/>'s character set.
        /// </summary>
        /// <param name="value">The string value to be read</param>
        /// <param name="baseChars">The character set of the base <paramref name="value"/> is written in</param>
        /// <returns>The numerical value of <paramref name="value"/> from base <paramref name="baseChars"/>.Length, using the characters from <paramref name="baseChars"/></returns>
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
