using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 错误描述类
    /// </summary>
    internal static class ErrorMessage
    {
        /// <summary>
        /// 发票代码为空
        /// </summary>
        public const string Empty = "发票代码为空";
        /// <summary>
        /// 错误的发票代码
        /// </summary>
        public const string Error = "错误的发票代码";
        /// <summary>
        /// 发票年份超出允许的年份范围
        /// </summary>
        public const string YearOutOfRange = "发票年份超出允许的年份范围{0} ~ {1}";
        /// <summary>
        /// 发票发行区域识别失败
        /// </summary>
        public const string InvalidArea = "发票发行区域识别失败";
        /// <summary>
        /// 无效的发票类别
        /// </summary>
        public const string InvalidKind = "无效的发票类别";
        /// <summary>
        /// 发票类别错误，无法生成发票代码
        /// </summary>
        public const string GenerateWrongKind = "发票类别错误，无法生成发票代码";
        /// <summary>
        /// 无效实现
        /// </summary>
        public const string InvalidImplement = "未能找到或无效的 {0} 位发票代码实现";
        /// <summary>
        /// 长度不符
        /// </summary>
        public const string LengthOutOfRange = "发票代码非 {0} 位";
    }
}
