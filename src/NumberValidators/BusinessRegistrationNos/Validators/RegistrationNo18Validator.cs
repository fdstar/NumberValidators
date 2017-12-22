using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// GB 32100-2015 法人和其他组织统一社会信用代码编码规则
    /// </summary>
    public class RegistrationNo18Validator
    {
        #region fields
        /// <summary>
        /// 本体代码及其对应的代码字符数值,不使用字母IOZSV
        /// </summary>
        internal static readonly Dictionary<char, int> CodeDictionary = new Dictionary<char, int>();
        /// <summary>
        /// 加权因子
        /// </summary>
        internal static readonly int[] WeightingFactors = { 1, 3, 9, 27, 19, 26, 16, 17, 20, 29, 25, 13, 8, 24, 10, 30, 28 };
        #endregion
        #region oct
        static RegistrationNo18Validator()
        {
            for (var i = 0; i <= 30; i++)
            {
                int num;
                if (i > 27) { num = 59; }
                else if (i > 25) { num = 58; }
                else if (i > 22) { num = 57; }
                else if (i > 17) { num = 56; }
                else if (i > 9) { num = 55; }
                else { num = 48; }
                var c = (char)(num + i);
                CodeDictionary.Add(c, i);
            }
        }
        #endregion
    }
}
