using NumberValidators.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税发票验证帮助类
    /// </summary>
    public static class VATCodeValidatorHelper
    {
        private static readonly ConcurrentDictionary<VATLength, IVATCodeValidator<VATCodeValidationResult>> concurrentDictionary
            = new ConcurrentDictionary<VATLength, IVATCodeValidator<VATCodeValidationResult>>();

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
            var valid = ValidatorHelper.ValidEmpty(vatCode, out VATCodeValidationResult result, ErrorMessage.Empty)
                && ValidatorHelper.ValidLength(vatCode, (int?)length, ErrorMessage.LengthOutOfRange, result)
                && ValidImplement(vatCode, result, out validator);
            return validator == null ? result : validator.Validate(vatCode, kind, minYear);
        }

        private static bool ValidImplement(string code, VATCodeValidationResult result, out IVATCodeValidator<VATCodeValidationResult> validator)
        {
            if (concurrentDictionary.Count > 0)
            {
                if (!concurrentDictionary.TryGetValue((VATLength)code.Length, out validator))
                {
                    result.AddErrorMessage(ErrorMessage.InvalidImplement, code.Length);
                }
            }
            else
            {
                ValidatorHelper.ValidImplement(code, result, "VATCode{0}Validator", ErrorMessage.InvalidImplement, out validator, typeof(IVATCodeValidator<>));
            }
            return validator != null;
        }

        /// <summary>
        /// 设置默认的校验规则，注意如果进行了设置，那么将不再进行自动推导，但会调用<see cref="AddDefaultValidator"/>进行默认设置
        /// </summary>
        /// <param name="vatLength">默认校验实现对应的编号长度</param>
        /// <param name="validator">默认实现</param>
        public static void SetValidator(VATLength vatLength, IVATCodeValidator<VATCodeValidationResult> validator)
        {
            if (concurrentDictionary.Count == 0)
            {
                AddDefaultValidator();
            }
            concurrentDictionary.AddOrUpdate(vatLength, k => null, (k, a) => validator);
        }
        /// <summary>
        /// 添加默认已提供的<see cref="VATLength"/>对应实现，用于临时解决core下可能会出现的反射错误
        /// </summary>
        public static void AddDefaultValidator()
        {
            concurrentDictionary.TryAdd(VATLength.Ten, new VATCode10Validator());
            concurrentDictionary.TryAdd(VATLength.Twelve, new VATCode12Validator());
        }
    }
}
