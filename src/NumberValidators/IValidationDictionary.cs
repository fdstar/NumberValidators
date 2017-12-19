using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators
{
    /// <summary>
    /// 验证用的基础数据字典接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IValidationDictionary<TKey, TValue>
    {
        /// <summary>
        /// 获取验证所需的字典
        /// </summary>
        /// <returns></returns>
        IDictionary<TKey, TValue> GetDictionary();
    }
}
