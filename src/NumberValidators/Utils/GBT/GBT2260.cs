using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Utils.GBT
{
    /// <summary>
    /// GBT2260标准，目前分别有以下10个版本：1980,1982,1984,1986,1988,1991,1995,1999,2002,2007 等等
    /// 第一代身份证办理时用的是1984版本……
    /// </summary>
    public abstract class GBT2260 : IValidationDictionary<int, string>
    {
        /// <summary>
        /// 获取数据字典
        /// </summary>
        /// <returns></returns>
        public abstract IDictionary<int, string> GetDictionary();
    }
}
