using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 工商注册号、统一社会信用代码验证帮助类
    /// </summary>
    public static class RegistrationNoValidatorHelper
    {
        /// <summary>
        /// 验证身份证是否正确
        /// </summary>
        /// <param name="code">工商注册号、统一社会信用代码</param>
        /// <param name="validLength">要验证的证件长度，默认不指定null</param>
        /// <param name="validLimit">验证区域级别，默认不指定null</param>
        /// <returns>验证结果</returns>
        public static RegistrationNoValidationResult Validate(this string code, RegistrationNoLength? validLength = null, AreaValidLimit? validLimit = null)
        {
            IRegistrationNoValidator<RegistrationNoValidationResult> validator = null;
            var valid = ValidatorHelper.ValidEmpty(code, out RegistrationNoValidationResult result, ErrorMessage.Empty)
                && ValidatorHelper.ValidLength(code, (int?)validLength, ErrorMessage.LengthOutOfRange, result)
                && ValidatorHelper.ValidImplement(code, result, "RegistrationNo{0}Validator", ErrorMessage.InvalidImplement, out validator, typeof(IRegistrationNoValidator<>));
            return validator == null ? result : validator.Validate(code, validLimit);
        }
    }
}
