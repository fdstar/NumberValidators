using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators
{
    /// <summary>
    /// 号码验证结果类
    /// </summary>
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Errors = new List<string>();
        }
        /// <summary>
        /// 验证结果是否通过
        /// </summary>
        public bool IsValid { get; internal set; } = true;
        /// <summary>
        /// 如果验证不通过，这里包含验证失败的原因
        /// </summary>
        public IList<string> Errors { get; internal set; }
        /// <summary>
        /// 当前验证的号码
        /// </summary>
        public string Number { get; internal set; }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="parameters"></param>
        internal void AddErrorMessage(string errorMsg, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                errorMsg = string.Format(errorMsg, parameters);
            }
            this.Errors.Add(errorMsg);
            this.IsValid = false;
        }
        /// <summary>
        /// 验证号码是否为空
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="number">要验证的号码</param>
        /// <param name="result">验证结果类</param>
        /// <param name="errorMsg">为空时的错误提示信息</param>
        /// <returns></returns>
        internal static bool ValidEmpty<T>(string number, out T result, string errorMsg)
            where T : ValidationResult, new()
        {
            result = new T()
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
