using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 错误提示信息类
    /// </summary>
    internal class ErrorMessage
    {
        /// <summary>
        /// 字符串为空
        /// </summary>
        public const string Empty = "工商注册码/统一社会信用代码为空";
        /// <summary>
        /// 号码错误
        /// </summary>
        public const string Error = "错误的工商注册码/统一社会信用代码";
        /// <summary>
        /// 无效的登记管理部门代码
        /// </summary>
        public const string InvalidManagement = "无效的登记管理部门代码";
        /// <summary>
        /// 无效的登记管理部门机构类别代码
        /// </summary>
        public const string InvalidManagementKind = "无效的登记管理部门机构类别代码";
        /// <summary>
        /// 无效的组织机构代码
        /// </summary>
        public const string InvalidOrganizationCode = "无效的组织机构代码";
        /// <summary>
        /// 行政区划识别失败
        /// </summary>
        public const string InvalidArea = "工商管理机关或行政区划识别失败";
        /// <summary>
        /// 错误的校验码
        /// </summary>
        public const string InvalidCheckBit = "错误的校验码";
        /// <summary>
        /// 无效实现
        /// </summary>
        public const string InvalidImplement = "未能找到或无效的 {0} 位工商注册码/统一社会信用代码实现";
        /// <summary>
        /// 长度错误
        /// </summary>
        public const string LengthOutOfRange = "工商注册码/统一社会信用代码非 {0} 位";
    }
}
