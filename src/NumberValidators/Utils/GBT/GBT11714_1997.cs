using NumberValidators.Utils.ISO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NumberValidators.Utils.GBT
{
    /// <summary>
    /// 国标GB/T 11714-1997
    /// </summary>
    public class GBT11714_1997
    {
        /// <summary>
        /// 加权因子
        /// </summary>
        public static readonly int[] WeightingFactors = { 3, 7, 9, 10, 5, 8, 4, 2 };
        /// <summary>
        /// 校验码
        /// </summary>
        public static readonly char[] CheckBits = new char[11];
        /// <summary>
        /// 本体代码及其对应的代码字符数值
        /// </summary>
        public static readonly Dictionary<char, int> CodeDictionary = new Dictionary<char, int>();
        static GBT11714_1997()
        {
            for (var i = 0; i <= 35; i++)
            {
                var c = (char)((i > 9 ? 55 : 48) + i);
                CodeDictionary.Add(c, i);
            }
            for (var i = 0; i < 11; i++)
            {
                char c = (char)(48 + i);
                if (i == 10) { c = 'X'; }
                CheckBits[i] = c;
            }
        }
        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <param name="code">组织机构代码前八位，注意必须大写字母或数字，如超出8位也只取前8位</param>
        /// <returns></returns>
        public static char GetCheckBit(string code)
        {
            if (code == null || !Regex.IsMatch(code, @"^[0-9A-Z]{8,}$"))
            {
                throw new ArgumentException("Error code.");
            }
            var mod = 11 - ISO7064_1983.MOD_11_2(code.Select(c => CodeDictionary[c]).Take(8).ToArray(), WeightingFactors, 11);
            return CheckBits[mod];
        }
    }
}
