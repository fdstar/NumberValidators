using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    internal class ErrorMessage
    {
        public const string Empty = "发票代码为空";
        public const string Error = "错误的发票代码";
        public const string YearOutOfRange = "发票年份超出允许的年份范围{0} ~ {1}";
        public const string InvalidArea = "发票发行区域识别失败";
        public const string InvalidKind = "无效的发票类别";
        public const string GenerateWrongKind = "发票类别错误，无法生成发票代码";
    }
}
