using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// GS15-2006 工商行政管理市场主体注册号编制规则
    /// </summary>
    public class RegistrationNo15Validator
    {
        private IValidationDictionary<int, string> _administrationDictionary;
        /// <summary>
        /// 默认的工商行政管理机关代码字典类
        /// </summary>
        protected IValidationDictionary<int, string> DefaultAdministrationDictionary { get; } = AdministrationValidationDictionary.Singleton;
        /// <summary>
        /// 工商行政管理机关代码字典类
        /// </summary>
        public IValidationDictionary<int, string> AdministrationDictionary
        {
            get
            {
                return this._administrationDictionary ?? DefaultAdministrationDictionary;
            }
            set
            {
                this._administrationDictionary = value;
            }
        }
        /// <summary>
        /// 工商行政管理机关代码
        /// </summary>
        internal class AdministrationValidationDictionary : IValidationDictionary<int, string>
        {
            private AdministrationValidationDictionary() { }
            /// <summary>
            /// 单例
            /// </summary>
            public static readonly AdministrationValidationDictionary Singleton = new AdministrationValidationDictionary();
            private static readonly Dictionary<int, string> Dictionary = new Dictionary<int, string>
            {
                { 100000,"国家工商行政管理总局"},
            };
            public IDictionary<int, string> GetDictionary()
            {
                return Dictionary;
            }
        }
    }
}
