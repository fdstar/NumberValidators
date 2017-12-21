using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos
{
    /// <summary>
    /// 注册码通用验证结果类
    /// </summary>
    public class RegistrationNoValidationResult : ValidationResult
    {
        /// <summary>
        /// 行政区划或工商行政管理机关编码
        /// </summary>
        public int AreaNumber { get; internal set; }
        /// <summary>
        /// 身份证颁发行政区域或工商行政管理机关（识别出Depth最深的区域），可通过FullName来获取完整的名称
        /// 注意此处有可能为null
        /// </summary>
        public Area RecognizableArea { get; internal set; }
        /// <summary>
        /// 校验码
        /// </summary>
        public char CheckBit { get; internal set; }
    }
}
