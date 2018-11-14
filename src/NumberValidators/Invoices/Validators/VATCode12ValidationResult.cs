using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 除增值税专项发票外的验证结果
    /// </summary>
    public class VATCode12ValidationResult : VATCodeValidationResult
    {
        /// <summary>
        /// 增值税电子发票细分类型
        /// </summary>
        public ElectronicVATKind? ElectronicVATKind { get; set; }
    }
}
