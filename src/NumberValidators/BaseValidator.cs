using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NumberValidators
{
    /// <summary>
    /// 基础验证类，默认包含了空验证和正则验证，提供默认的错误信息描述
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValidator<T> : IValidator<T>
        where T : ValidationResult, new()
    {
        /// <summary>
        /// 用于判断输入号码是否正确的正则表达式，如果不设定，则不进行正则匹配
        /// </summary>
        protected virtual string RegexPattern { get; }
        /// <summary>
        /// 号码为空时的错误信息
        /// </summary>
        protected virtual string EmptyErrorMessage { get; } = "号码不能为空";
        /// <summary>
        /// 正则匹配失败时的错误信息
        /// </summary>
        protected virtual string RegexMatchFailMessage { get; } = "号码错误";
        /// <summary>
        /// 生成随机号码
        /// </summary>
        /// <returns></returns>
        public abstract string GenerateRandomNumber();
        /// <summary>
        /// 验证号码是否正确
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public virtual T Validate(string number)
        {
            var valid = ValidationResult.ValidEmpty(number, out T result, EmptyErrorMessage)
                && this.ValidWithPattern(number, result);
            return result;
        }
        /// <summary>
        /// 按正则验证号码是否正确
        /// </summary>
        /// <param name="number"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidWithPattern(string number, T result)
        {
            bool valid = string.IsNullOrEmpty(this.RegexPattern)|| Regex.IsMatch(number, this.RegexPattern);
            if (!valid)
            {
                result.AddErrorMessage(this.RegexMatchFailMessage);
            }
            return valid;
        }
    }
}
