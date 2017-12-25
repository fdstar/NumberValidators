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
        /// <summary>
        /// 验证结果是否通过
        /// </summary>
        public bool IsValid { get; internal set; } = true;
        /// <summary>
        /// 如果验证不通过，这里包含验证失败的原因
        /// </summary>
        public IList<string> Errors { get; internal set; } = new List<string>();
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
    }
}
