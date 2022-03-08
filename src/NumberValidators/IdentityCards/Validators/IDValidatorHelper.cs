using NumberValidators.Utils;
using System;
using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<IDLength, IIDValidator> concurrentDictionary
            = new ConcurrentDictionary<IDLength, IIDValidator>();

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
            _ = ValidatorHelper.ValidEmpty(idNumber, out IDValidationResult result, ErrorMessage.Empty)
                && ValidatorHelper.ValidLength(idNumber, (int?)validLength, ErrorMessage.LengthOutOfRange, result)
                && ValidImplement(idNumber, result, out validator);
            return validator == null ? result : validator.Validate(idNumber, minYear, validLimit, ignoreCheckBit);
        }

        private static bool ValidImplement(string code, IDValidationResult result, out IIDValidator validator)
        {
            if (concurrentDictionary.Count > 0)
            {
                if (!concurrentDictionary.TryGetValue((IDLength)code.Length, out validator))
                {
                    result.AddErrorMessage(ErrorMessage.InvalidImplement, code.Length);
                }
            }
            else
            {
                ValidatorHelper.ValidImplement(code, result, "ID{0}Validator", ErrorMessage.InvalidImplement, out validator, typeof(IIDValidator));
            }
            return validator != null;
        }

        /// <summary>
        /// 设置默认的校验规则，注意如果进行了设置，那么将不再进行自动推导，但会调用<see cref="AddDefaultValidator"/>进行默认设置
        /// </summary>
        /// <param name="idLength">默认校验实现对应的编号长度</param>
        /// <param name="validator">默认实现</param>
        public static void SetValidator(IDLength idLength, IIDValidator validator)
        {
            if (concurrentDictionary.Count == 0)
            {
                AddDefaultValidator();
            }
            concurrentDictionary.AddOrUpdate(idLength, k => null, (k, a) => validator);
        }
        /// <summary>
        /// 添加默认已提供的<see cref="IDLength"/>对应实现，用于临时解决core下可能会出现的反射错误
        /// </summary>
        public static void AddDefaultValidator()
        {
            concurrentDictionary.TryAdd(IDLength.Fifteen, new ID15Validator());
            concurrentDictionary.TryAdd(IDLength.Eighteen, new ID18Validator());
        }
    }
}
