using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices
{
    /// <summary>
    /// 金额版本号，仅10位发票代码有
    /// </summary>
    public enum AmountVersion
    {
        /// <summary>
        /// 电脑开票
        /// </summary>
        Computer = 0,
        /// <summary>
        /// 手写万元版
        /// </summary>
        TenThousand = 1,
        /// <summary>
        /// 手写十万元版
        /// </summary>
        OneHundredThousand = 2,
        /// <summary>
        /// 手写百万元版
        /// </summary>
        Million = 3,
        /// <summary>
        /// 手写千万元版
        /// </summary>
        TenMillion = 4
    }
}
