using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators
{
    public interface IValidationDictionary<TKey, TValue>
    {
        /// <summary>
        /// 获取验证所需的字典
        /// </summary>
        /// <returns></returns>
        IDictionary<TKey, TValue> GetDictionary();
    }
}
