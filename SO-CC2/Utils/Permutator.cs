using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SO_CC2.Utils
{
    public static class Permutator
    {
        /// <summary>
        /// Calculates the frequency of each character contained in <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The string to read</param>
        /// <returns>A dictionary containing the frequency of each character contained in <paramref name="str"/></returns>
        private static Dictionary<char, int> GetFrequencies(string str)
        {
            Dictionary<char, int> counts = new();

            foreach (char c in str)
            {
                if (counts.ContainsKey(c))
                    counts[c]++;
                else
                    counts.Add(c, 1);
            }

            return counts;
        }

        /// <summary>
        /// Gets the permutation index of <paramref name="str"/>, using its lexicographically sorted version as a base.
        /// <br/>
        /// Acts as the reverse of <see cref="IndexToPermutation"/>
        /// </summary>
        /// <remarks>
        /// Written using https://math.stackexchange.com/a/3803239
        /// </remarks>
        /// <param name="str">The string to get the permutation index of</param>
        /// <returns>The permutation index of <paramref name="str"/></returns>
        public static BigInteger PermutationToIndex(string str)
        {
            BigInteger rank = 0;
            Dictionary<char, int> freqs = GetFrequencies(str);

            int minOrd = freqs.Keys.Min();

            for (int n = 0; n < str.Length; n++)
            {
                int fsum = freqs.Where((kvp) => kvp.Key >= minOrd && kvp.Key < str[n]).Sum(kvp => kvp.Value);
                BigInteger fprod = freqs.Values.Select(v => MathHelper.Factorial(v)).Aggregate(BigInteger.One, (x, y) => x * y);
                freqs[str[n]] -= 1;
                rank += ((fsum * MathHelper.Factorial(str.Length - n - 1)) / fprod);
            }

            return rank;
        }

        /// <summary>
        /// Computes the <paramref name="index"/>-th permutation of <paramref name="baseStr"/>.
        /// <br/>
        /// Acts as the reverse of <see cref="PermutationToIndex"/>
        /// </summary>
        /// <remarks>
        /// Written using https://stackoverflow.com/a/24508736/9399492
        /// </remarks>
        /// <param name="baseStr">The string to permutate (automatically sorted lexicographically)</param>
        /// <param name="index">The permutation index</param>
        /// <returns>The <paramref name="index"/>-th permutation of <paramref name="baseStr"/></returns>
        public static string IndexToPermutation(string baseStr, BigInteger index)
        {
            baseStr = new string(baseStr.ToString().OrderBy(x => x).ToArray());

            if (index == 0)
                return baseStr;

            Dictionary<char, int> freqs = GetFrequencies(baseStr);
            BigInteger totalCount = MathHelper.Factorial(freqs.Values.Sum()) / freqs.Values.Aggregate(BigInteger.One, (a, v) => a * MathHelper.Factorial(v));
            BigInteger acc = 0;

            for (int n = 0; n < baseStr.Length; n++)
            {
                if (n > 0 && baseStr[n] == baseStr[n - 1])
                    continue;

                BigInteger count = totalCount * freqs[baseStr[n]] / baseStr.Length;

                if (acc + count > index)
                    return baseStr[n] + IndexToPermutation(baseStr.Remove(n, 1), index - acc);

                acc += count;
            }

            return "";
        }
    }
}
