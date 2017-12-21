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
        public char ManagementCode { get; internal set; }
        /// <summary>
        /// 登记管理部门名称
        /// </summary>
        public string  ManagementName { get; internal set; }
        /// <summary>
        /// 登记管理部门下的组织机构代码
        /// </summary>
        public char ManagementOrgCode { get; internal set; } = '1';
        /// <summary>
        /// 登记管理部门下的组织机构名称
        /// </summary>
        public string ManagementOrgName { get; internal set; } = string.Empty;
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrganizationCode { get; internal set; }
    }
}
