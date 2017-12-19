using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    /// <summary>
    /// 错误提示信息类
    /// </summary>
    internal class ErrorMessage
    {
        /// <summary>
        /// 身份证号码为空
        /// </summary>
        public const string Empty = "身份证号码为空";
        /// <summary>
        /// 错误的身份证号码
        /// </summary>
        public const string Error = "错误的身份证号码";
        /// <summary>
        /// 无效的出生日期
        /// </summary>
        public const string InvalidBirthday = "无效的出生日期";
        /// <summary>
        /// 出生日期超出允许的年份范围
        /// </summary>
        public const string BirthdayYearOutOfRange = "出生日期超出允许的年份范围{0} ~ {1}";
        /// <summary>
        /// 行政区划识别失败
        /// </summary>
        public const string InvalidArea = "行政区划识别失败";
        /// <summary>
        /// 行政区划识别度不足
        /// </summary>
        public const string AreaLimitOutOfRange = "行政区划识别度低于识别级别 {0}";
        /// <summary>
        /// 错误的校验码
        /// </summary>
        public const string InvalidCheckBit = "错误的校验码";
        /// <summary>
        /// 无效实现
        /// </summary>
        public const string InvalidImplement = "未能找到或无效的 {0} 位身份证实现";
        /// <summary>
        /// 长度错误
        /// </summary>
        public const string LengthOutOfRange = "身份证号码非 {0} 位";
    }
}
