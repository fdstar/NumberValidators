using NumberValidators.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 工商注册号、统一社会信用代码验证帮助类
    /// </summary>
    public static class RegistrationNoValidatorHelper
    {
        private static readonly ConcurrentDictionary<RegistrationNoLength, IRegistrationNoValidator<RegistrationNoValidationResult>> concurrentDictionary
            = new ConcurrentDictionary<RegistrationNoLength, IRegistrationNoValidator<RegistrationNoValidationResult>>();
        /// <summary>
        /// 验证工商注册号、统一社会信用代码是否正确
        /// </summary>
        /// <param name="code">工商注册号、统一社会信用代码</param>
        /// <param name="validLength">要验证的证件长度，默认不指定null</param>
        /// <param name="validLimit">验证区域级别，默认不指定null</param>
        /// <returns>验证结果</returns>
        public static RegistrationNoValidationResult Validate(this string code, RegistrationNoLength? validLength = null, AreaValidLimit? validLimit = null)
        {
            IRegistrationNoValidator<RegistrationNoValidationResult> validator = null;
            _ = ValidatorHelper.ValidEmpty(code, out RegistrationNoValidationResult result, ErrorMessage.Empty)
                && ValidatorHelper.ValidLength(code, (int?)validLength, ErrorMessage.LengthOutOfRange, result)
                && ValidImplement(code, result, out validator);
            return validator == null ? result : validator.Validate(code, validLimit);
        }

        private static bool ValidImplement(string code, RegistrationNoValidationResult result, out IRegistrationNoValidator<RegistrationNoValidationResult> validator)
        {
            if (concurrentDictionary.Count > 0)
            {
                if (!concurrentDictionary.TryGetValue((RegistrationNoLength)code.Length, out validator))
                {
                    result.AddErrorMessage(ErrorMessage.InvalidImplement, code.Length);
                }
            }
            else
            {
                ValidatorHelper.ValidImplement(code, result, "RegistrationNo{0}Validator", ErrorMessage.InvalidImplement, out validator, typeof(IRegistrationNoValidator<>));
            }
            return validator != null;
        }

        /// <summary>
        /// 设置默认的校验规则，注意如果进行了设置，那么将不再进行自动推导，但会调用<see cref="ResetDefaultValidator"/>进行默认设置
        /// </summary>
        /// <param name="noLength">默认校验实现对应的编号长度</param>
        /// <param name="validator">默认实现</param>
        public static void SetValidator(RegistrationNoLength noLength, IRegistrationNoValidator<RegistrationNoValidationResult> validator)
        {
            if (concurrentDictionary.Count == 0)
            {
                ResetDefaultValidator();
            }
            concurrentDictionary.AddOrUpdate(noLength, k => null, (k, a) => validator);
        }
        /// <summary>
        /// 添加或重置默认已提供的<see cref="RegistrationNoLength"/>对应实现，用于临时解决core下可能会出现的反射错误
        /// </summary>
        public static void ResetDefaultValidator()
        {
            concurrentDictionary.AddOrUpdate(RegistrationNoLength.Fifteen, k => new RegistrationNo15Validator(), (k, a) => new RegistrationNo15Validator());
            concurrentDictionary.AddOrUpdate(RegistrationNoLength.Eighteen, k => new RegistrationNo18Validator(), (k, a) => new RegistrationNo18Validator());
        }
    }
}
