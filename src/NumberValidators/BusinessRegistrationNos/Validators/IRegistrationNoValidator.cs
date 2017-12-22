using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 工商注册码/统一社会信用代码验证接口
    /// </summary>
    public interface IRegistrationNoValidator<out TResult> : IValidator<TResult>
        where TResult : RegistrationNoValidationResult, new()
    {
        /// <summary>
        /// 用于验证的行政区划字典数据
        /// </summary>
        IValidationDictionary<int, string> Dictionary { get; set; }
        /// <summary>
        /// 号码长度
        /// </summary>
        RegistrationNoLength RegistrationNoLength { get; }
        /// <summary>
        /// 验证号码是否正确
        /// </summary>
        /// <param name="code">待验证的工商注册码/统一社会信用代码</param>
        /// <param name="validLimit">行政区划验证限制，因为存在工商管理机构代码，所以默认为null</param>
        /// <returns></returns>
        TResult Validate(string code, AreaValidLimit? validLimit = null);
    }
}
