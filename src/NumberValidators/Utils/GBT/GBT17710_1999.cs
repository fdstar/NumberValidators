using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Utils.GBT
{
    /// <summary>
    /// 国标GB/T 17710-1999
    /// </summary>
    public class GBT17710_1999
    {
        /// <summary>
        /// 按GB/T 17710-1999中定义的ISO 7064 MOD 11,10算法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int MOD_11_10(int[] source)
        {
            int p = 10;
            for (var i = 0; i < source.Length; i++)
            {
                int s = (source[i] + p) % 10 * 2;
                p = (s == 0 ? 20 : s) % 11;
            }
            return (11 - p) % 10;
        }
    }
}
