using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税发票和普通（纸质）专有的验证结果
    /// </summary>
    public class VATCode10ValidationResult : VATCodeValidationResult
    {
        /// <summary>
        /// 发票金额版本号，仅10位长度发票才有
        /// </summary>
        public AmountVersion AmountVersion { get; internal set; }
    }
}
