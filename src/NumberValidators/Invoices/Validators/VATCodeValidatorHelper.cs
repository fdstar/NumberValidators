using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税发票验证帮助类
    /// </summary>
    public static class VATCodeValidatorHelper
    {
        /// <summary>
        /// 验证当前增值税发票代码是否正确
        /// </summary>
        /// <param name="vatCode">发票代码</param>
        /// <param name="kind">发票类型</param>
        /// <param name="length">发票长度</param>
        /// <param name="minYear">最小年份</param>
        /// <returns></returns>
        public static VATCodeValidationResult Validate(string vatCode, VATKind? kind = null, VATLength? length = null, ushort minYear = 2012)
        {
            IVATCodeValidator<VATCodeValidationResult> validator = null;
            var valid = ValidationResult.ValidEmpty(vatCode, out VATCodeValidationResult result, ErrorMessage.Empty)
                && ValidVATLength(vatCode, length, result)
                && ValidImplement(vatCode, result, out validator);
            return validator == null ? result : validator.Validate(vatCode, kind, minYear);
        }
        private static bool ValidImplement(string vatCode, VATCodeValidationResult result, out IVATCodeValidator<VATCodeValidationResult> validator)
        {
            validator = ReflectionHelper.FindByInterface(typeof(IVATCodeValidator<>), string.Format("VATCode{0}Validator", vatCode.Length)) as IVATCodeValidator<VATCodeValidationResult>;
            var valid = validator != null;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidImplement, vatCode.Length);
            }
            return valid;
        }
        private static bool ValidVATLength(string vatCode, VATLength? validLength, VATCodeValidationResult result)
        {
            bool valid = !validLength.HasValue || vatCode.Length == (int)validLength;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.LengthOutOfRange, (int)validLength);
            }
            return valid;
        }
    }
}
