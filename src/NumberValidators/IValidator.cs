using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators
{
    /// <summary>
    /// 所有号码验证类均需实现的基础接口定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidator<out T>
        where T : ValidationResult, new()
    {
        /// <summary>
        /// 随机生成一个符合规则的号码
        /// </summary>
        /// <returns></returns>
        string GenerateRandomNumber();
        /// <summary>
        /// 验证号码是否正确
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        T Validate(string number);
    }
}
