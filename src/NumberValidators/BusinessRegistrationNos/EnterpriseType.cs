using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos
{
    /// <summary>
    /// 企业类型
    /// </summary>
    public enum EnterpriseType
    {
        /// <summary>
        /// 内资企业 0123
        /// </summary>
        Domestic = 3,
        /// <summary>
        /// 外资企业 45
        /// </summary>
        Foreign = 5,
        /// <summary>
        /// 个体工商户 6789
        /// </summary>
        Individual = 9
    }
}
