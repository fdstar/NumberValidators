using NumberValidators.Utils.ISO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    /// <summary>
    /// 18位（新）身份证校验规则
    /// </summary>
    public class ID18Validator : IDValidator
    {
        /*
        18位身份证号码各位数字的含义:  
        1-2位省、自治区、直辖市代码； 
        3-4位地级市、盟、自治州代码； 
        5-6位县、县级市、区代码；  
        7-14位出生年月日，比如19670401代表1967年4月1日，遵循GB/T 7408-1994标准； 
        15-17位为顺序号，其中17位男为单数，女为双数； 
        18位为校验码，0-9和X，由公式随机产生。 举例：  130503196704010012这个身份证号的含义:13为河北，05为邢台，03为桥西区，出生日期为1967年4月1日，顺序号为001，2为验证码。  
        */
        /// <summary>
        /// 证件长度
        /// </summary>
        public override IDLength IDLength => IDLength.Eighteen;
        /// <summary>
        /// 验证用的正则表达式
        /// </summary>
        protected override string RegexPattern => @"^[1-8]\d{16}[0-9xX]$";
        /// <summary>
        /// 生成身份证
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="birthDay"></param>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        protected override string GenerateID(string areaNumber, DateTime birthDay, string sequenceNumber)
        {
            var tmp = string.Format("{0}{1:yyyyMMdd}{2}", areaNumber, birthDay, sequenceNumber);
            var checkBit = this.GetCheckBit(tmp);
            return string.Format("{0}{1}", tmp, checkBit);
        }
        /// <summary>
        /// 获取出生日期
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        protected override DateTime GetBirthday(string idNumber)
        {
            DateTime.TryParseExact(idNumber.Substring(6, 8), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            return date;
        }
        /// <summary>
        /// 获取登记序列号
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        protected override string GetSequenceNumber(string idNumber)
        {
            return idNumber.Substring(14, 3);
        }
        /// <summary>
        /// 校验码加权因子
        /// </summary>
        private static readonly int[] WeightingFactors = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        /// <summary>
        /// 按照ISO7064:1983.MOD取模结果对应的校验码(对应数组索引)
        /// </summary>
        private static readonly char[] CheckBits = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
        /// <summary>
        /// 校验位是否正确
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="rightBit"></param>
        /// <returns></returns>
        protected override bool IsCheckBitRight(string idNumber, out char rightBit)
        {
            rightBit = GetCheckBit(idNumber);
            return rightBit == char.ToUpper(idNumber[17]);
        }
        private char GetCheckBit(string idNumber)
        {
            var mod = ISO7064_1983.MOD_11_2(idNumber.Select(c => (int)c - 48).Take(17).ToArray(), WeightingFactors, 11);
            return CheckBits[mod];
        }
    }
}
