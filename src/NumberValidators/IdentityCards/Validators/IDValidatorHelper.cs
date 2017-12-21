using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    /// <summary>
    /// 身份证验证帮助类
    /// </summary>
    public static class IDValidatorHelper
    {
        /// <summary>
        /// 身份证升位，如果返回false表示升位失败，输入的旧身份证号码不正确
        /// </summary>
        /// <param name="oldID">15位身份证号码</param>
        /// <param name="newID">18位身份证号码</param>
        /// <returns></returns>
        public static bool TryPromotion(this string oldID, out string newID)
        {
            newID = null;
            var valid = new ID15Validator().Validate(oldID);
            if (valid.IsValid)
            {
                newID = new ID18Validator().GenerateID(valid.AreaNumber, valid.Birthday, valid.Sequence);
            }
            return valid.IsValid;
        }
        /// <summary>
        /// 验证身份证是否正确
        /// </summary>
        /// <param name="idNumber">待验证的证件号码</param>
        /// <param name="minYear">允许最小年份，默认0</param>
        /// <param name="validLength">要验证的证件长度，默认不指定null</param>
        /// <param name="validLimit">验证区域级别，默认AreaValidLimit.Province</param>
        /// <param name="ignoreCheckBit">是否忽略校验位验证，默认false</param>
        /// <returns>验证结果</returns>
        public static IDValidationResult Validate(this string idNumber, ushort minYear = 0, IDLength? validLength = null, AreaValidLimit validLimit = AreaValidLimit.Province, bool ignoreCheckBit = false)
        {
            IIDValidator validator = null;
            var valid = ValidationResult.ValidEmpty(idNumber, out IDValidationResult result, ErrorMessage.Empty)
                && ValidIDLength(idNumber, validLength, result)
                && ValidImplement(idNumber, result, out validator);
            return validator == null ? result : validator.Validate(idNumber, minYear, validLimit, ignoreCheckBit);
        }
        private static bool ValidImplement(string idNumber, IDValidationResult result, out IIDValidator validator)
        {
            validator = ReflectionHelper.FindByInterface<IIDValidator>(string.Format("ID{0}Validator", idNumber.Length));
            var valid = validator != null;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidImplement, idNumber.Length);
            }
            return valid;
        }
        private static bool ValidIDLength(string idNumber, IDLength? validLength, IDValidationResult result)
        {
            bool valid = !validLength.HasValue || idNumber.Length == (int)validLength;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.LengthOutOfRange, (int)validLength);
            }
            return valid;
        }
    }
}
