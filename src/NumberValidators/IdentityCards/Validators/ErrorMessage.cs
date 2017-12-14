using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    internal class ErrorMessage
    {
        public const string Empty = "身份证号码为空";
        public const string Error = "错误的身份证号码";
        public const string InvalidBirthday = "无效的出生日期";
        public const string BirthdayYearOutOfRange = "出生日期超出允许的年份范围{0} ~ {1}";
        public const string InvalidArea = "行政区划识别失败";
        public const string AreaLimitOutOfRange = "行政区划识别度低于识别级别 {0}";
        public const string InvalidCheckBit = "错误的校验码";
        public const string InvalidImplement = "未能找到或无效的 {0} 位身份证实现";
        public const string LengthOutOfRange = "身份证号码非 {0} 位";
    }
}
