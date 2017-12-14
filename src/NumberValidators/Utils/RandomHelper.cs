using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Utils
{
    public static class RandomHelper
    {
        public static int GetRandomNumber(this int seed)
        {
            return Math.Abs(Guid.NewGuid().GetHashCode()) % seed;
        }
    }
}
