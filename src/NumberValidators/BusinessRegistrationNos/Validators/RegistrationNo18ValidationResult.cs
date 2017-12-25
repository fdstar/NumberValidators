using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 法人和其他组织统一社会信用代码 专用验证结果类
    /// </summary>
    public class RegistrationNo18ValidationResult : RegistrationNoValidationResult
    {
        /// <summary>
        /// 登记管理部门代码标志
        /// </summary>
        public ManagementCode ManagementCode { get; internal set; }
        /// <summary>
        /// 登记管理部门下机构类别代码标志
        /// </summary>
        public ManagementKindCode ManagementKindCode { get; internal set; } = ManagementKindCode.NonSpecific;
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrganizationCode { get; internal set; }
    }
}
