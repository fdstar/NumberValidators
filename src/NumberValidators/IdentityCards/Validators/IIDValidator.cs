using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    /// <summary>
    /// 身份证验证接口
    /// </summary>
    public interface IIDValidator : IValidator<IDValidationResult>
    {
        /// <summary>
        /// 用于验证的字典数据
        /// </summary>
        IValidationDictionary<int, string> Dictionary { get; set; }
        /// <summary>
        /// 生成身份证号码
        /// </summary>
        /// <param name="areaNumber">行政区划编号</param>
        /// <param name="birthDay">出生日期</param>
        /// <param name="sequenceNumber">顺序号</param>
        /// <returns></returns>
        string GenerateID(int areaNumber, DateTime birthDay, int sequenceNumber);
        /// <summary>
        /// 验证身份证是否正确
        /// </summary>
        /// <param name="idNumber">待验证的证件号码</param>
        /// <param name="minYear">允许最小年份，默认0</param>
        /// <param name="validLimit">验证区域级别，默认AreaValidLimit.Province</param>
        /// <param name="ignoreCheckBit">是否忽略校验位验证，默认false</param>
        /// <returns>验证结果</returns>
        IDValidationResult Validate(string idNumber, ushort minYear = 0, AreaValidLimit validLimit = AreaValidLimit.Province, bool ignoreCheckBit = false);
    }
}
