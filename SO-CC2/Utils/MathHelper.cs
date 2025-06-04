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
        public static BigInteger Factorial(BigInteger n)
        {
            BigInteger result = 1;

            for (BigInteger i = n; i > 0; i--)
                result *= i;

            return result;
        }
    }
}
