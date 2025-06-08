using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SO_CC2.Utils
{
    public static class MathHelper
    {
        /// <summary>
        /// Calculates the factorial of <paramref name="n"/>.
        /// </summary>
        /// <param name="n">Integer to calculate the factorial of</param>
        /// <returns><paramref name="n"/> * (<paramref name="n"/> - 1) * (<paramref name="n"/> - 2) * ... * 1</returns>
        public static BigInteger Factorial(BigInteger n)
        {
            BigInteger result = 1;

            for (BigInteger i = 2; i <= n; i++)
                result *= i;

            return result;
        }
    }
}
