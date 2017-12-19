using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    /// <summary>
    /// 15位旧身份证校验规则
    /// </summary>
    public class ID15Validator : IDValidator
    {
        /*
        15位身份证号码各位的含义:  
        1-2位省、自治区、直辖市代码； 
        3-4位地级市、盟、自治州代码； 
        5-6位县、县级市、区代码；  
        7-12位出生年月日,比如670401代表1967年4月1日,这是和18位号码的第一个区别； 
        13-15位为顺序号，其中15位男为单数，女为双数； 
        与18位身份证号的第二个区别：没有最后一位的验证码。 

        15位身份证号码升级成18位身份证号码时，并不会修正GB/T 2260发生变化导致的区域编号变更，所以如果按最新的GBT2260标准进行身份证校验，会导致有些正确的身份证校验失败
        */
        /// <summary>
        /// 证件长度
        /// </summary>
        public override IDLength IDLength => IDLength.Fifteen;
        /// <summary>
        /// 验证用的正则表达式
        /// </summary>
        protected override string RegexPattern => @"^[1-8]\d{14}$";
        /// <summary>
        /// 生成身份证
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="birthDay"></param>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        protected override string GenerateID(string areaNumber, DateTime birthDay, string sequenceNumber)
        {
            return string.Format("{0}{1:yyMMdd}{2}", areaNumber, birthDay, sequenceNumber);
        }
        /// <summary>
        /// 获取出生日期
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        protected override DateTime GetBirthday(string idNumber)
        {
            DateTime.TryParseExact("19" + idNumber.Substring(6, 6), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            return date;
        }
        /// <summary>
        /// 获取登记序列号
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        protected override string GetSequenceNumber(string idNumber)
        {
            return idNumber.Substring(12, 3);
        }
        /// <summary>
        /// 校验位是否正确
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="rightBit"></param>
        /// <returns></returns>
        internal protected override bool IsCheckBitRight(string idNumber, out char rightBit)
        {
            rightBit = char.MinValue;
            return true;
        }
    }
}
