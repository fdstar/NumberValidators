using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Utils
{
    /// <summary>
    /// 随机数生成帮助类
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 按种子生成随机数
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static int GetRandomNumber(this int seed)
        {
            return Math.Abs(Guid.NewGuid().GetHashCode()) % seed;
        }
    }
}
