using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards
{
    /// <summary>
    /// 身份证验证结果类
    /// </summary>
    public class IDValidationResult : ValidationResult
    {
        /// <summary>
        /// 身份证号码长度
        /// </summary>
        public IDLength IDLength { get; internal set; }
        /// <summary>
        /// 身份证上的出生日期
        /// </summary>
        public DateTime Birthday { get; internal set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; internal set; }
        /// <summary>
        /// 行政区划编码
        /// </summary>
        public int AreaNumber { get; set; }
        /// <summary>
        /// 身份证颁发行政区域（识别出Depth最深的区域），可通过FullName来获取完整的区域名
        /// </summary>
        public Area RecognizableArea { get; internal set; }
        /// <summary>
        /// 出生登记顺序号
        /// </summary>
        public int Sequence { get; internal set; }
        /// <summary>
        /// 身份证校验码
        /// </summary>
        public char CheckBit { get; internal set; }
    }
}
