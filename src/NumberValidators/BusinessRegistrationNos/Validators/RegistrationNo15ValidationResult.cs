using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 工商行政管理市场主体注册号 专用验证结果类
    /// </summary>
    public class RegistrationNo15ValidationResult : RegistrationNoValidationResult
    {
        /// <summary>
        /// 顺序码
        /// </summary>
        public int SequenceNumber { get; internal set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        public EnterpriseType EnterpriseType
        {
            get
            {
                var comp = this.SequenceNumber / 10000000;
                if (comp <= (int)EnterpriseType.Domestic)
                {
                    return EnterpriseType.Domestic;
                }
                else if (comp <= (int)EnterpriseType.Foreign)
                {
                    return EnterpriseType.Foreign;
                }
                else { return EnterpriseType.Individual; }
            }
        }
    }
}
