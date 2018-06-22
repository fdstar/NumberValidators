using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices
{
    /// <summary>
    /// 增值税发票代码验证结果
    /// </summary>
    public class VATCodeValidationResult : ValidationResult
    {
        /// <summary>
        /// 行政区划代码
        /// </summary>
        public int AreaNumber { get; internal set; }
        /// <summary>
        /// 行政区域名称
        /// </summary>
        public string AreaName { get; internal set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        public VATKind? Category { get; internal set; }
        /// <summary>
        /// 印刷年份
        /// </summary>
        public int Year { get; internal set; }
        /// <summary>
        /// 印刷批次
        /// </summary>
        public int Batch { get; internal set; }
        /// <summary>
        /// 发票联次，仅10位长度和12位长度折叠票发票才有
        /// </summary>
        public int DuplicateNumber { get; internal set; }
    }
}
