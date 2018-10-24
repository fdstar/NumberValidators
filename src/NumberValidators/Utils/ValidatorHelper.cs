using System;

namespace NumberValidators.Utils
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public static class ValidatorHelper
    {
        /// <summary>
        /// 获取对应长度号码验证类实现
        /// </summary>
        /// <typeparam name="TResult">验证结果类</typeparam>
        /// <typeparam name="T">验证接口</typeparam>
        /// <param name="number">待验证的号码</param>
        /// <param name="result">已有的验证结果类</param>
        /// <param name="classNameFormat">要查找的className的格式，注意占位符{0}为number.Length，如不按照此规则，则不要传入占位符</param>
        /// <param name="errorMsgFormat">未能找到实现时的错误描述，注意占位符{0}为number.Length，如不按照此规则，则不要传入占位符</param>
        /// <param name="validator">验证接口实现类</param>
        /// <param name="interfaceType">要查找的接口类型，传null代表默认搜索typeof(IValidator)</param>
        /// <returns></returns>
        public static bool ValidImplement<TResult, T>(string number, TResult result, string classNameFormat, string errorMsgFormat, out T validator, Type interfaceType = null)
            where TResult : ValidationResult, new()
            where T : class, IValidator<TResult>
        {
            validator = ReflectionHelper.FindByInterface(interfaceType ?? typeof(IValidator<>), string.Format(classNameFormat, number.Length)) as T;
            var valid = validator != null;
            if (!valid)
            {
                result.AddErrorMessage(errorMsgFormat, number.Length);
            }
            return valid;
        }
        /// <summary>
        /// 验证号码长度是否符合希望验证的长度
        /// </summary>
        /// <typeparam name="TResult">验证结果类</typeparam>
        /// <param name="number">待验证的号码</param>
        /// <param name="validLength">需验证的长度，不验证则传null</param>
        /// <param name="errorMsgFormat">长度不符时的错误描述，注意占位符{0}为number.Length，如不按照此规则，则不要传入占位符</param>
        /// <param name="result">验证结果类</param>
        /// <returns></returns>
        public static bool ValidLength<TResult>(string number, int? validLength, string errorMsgFormat, TResult result)
            where TResult : ValidationResult, new()
        {
            bool valid = !validLength.HasValue || number.Length == validLength;
            if (!valid)
            {
                result.AddErrorMessage(errorMsgFormat, validLength);
            }
            return valid;
        }
        /// <summary>
        /// 验证号码是否为空
        /// </summary>
        /// <typeparam name="TResult">泛型</typeparam>
        /// <param name="number">要验证的号码</param>
        /// <param name="result">验证结果类</param>
        /// <param name="errorMsg">为空时的错误提示信息</param>
        /// <returns></returns>
        public static bool ValidEmpty<TResult>(string number, out TResult result, string errorMsg)
            where TResult : ValidationResult, new()
        {
            result = new TResult()
            {
                Number = number
            };
            if (string.IsNullOrWhiteSpace(number))
            {
                result.AddErrorMessage(errorMsg);
            }
            return result.IsValid;
        }
    }
}
