using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// GS15-2006 工商行政管理市场主体注册号编制规则
    /// </summary>
    public class RegistrationNo15Validator : RegistrationNoValidator<RegistrationNo15ValidationResult>
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
        /// 号码长度
        /// </summary>
        public override RegistrationNoLength RegistrationNoLength => RegistrationNoLength.Fifteen;
        /// <summary>
        /// 验证用正则
        /// </summary>
        protected override string RegexPattern => RegexPatterns.BusinessRegistrationNo;
        /// <summary>
        /// 工商行政管理机关代码
        /// </summary>
        private class AdministrationValidationDictionary : IValidationDictionary<int, string>
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
        /// <summary>
        /// 生成工商注册号
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <returns></returns>
        protected override string GenerateRegistrationNo(string areaNumber)
        {
            return this.GenerateRegistrationNo(areaNumber, RandomHelper.GetRandomNumber(10));
        }
        /// <summary>
        /// 生成工商注册码
        /// </summary>
        /// <param name="areaNumber">行政区划</param>
        /// <param name="enterpriseType">企业类型</param>
        /// <returns></returns>
        public string GenerateRegistrationNo(int areaNumber, EnterpriseType enterpriseType)
        {
            var typeSeed = 4;
            if (enterpriseType == EnterpriseType.Foreign)
            {
                typeSeed = 2;
            }
            var rdType = (int)enterpriseType - RandomHelper.GetRandomNumber(typeSeed);
            return this.GenerateRegistrationNo(areaNumber.ToString().PadRight(6, '0').Substring(0, 6), rdType);
        }
        private string GenerateRegistrationNo(string areaNumber, int rdType)
        {
            var seqNumber = RandomHelper.GetRandomNumber(10000000).ToString().PadLeft(7, '0');
            var tmp = string.Format("{0}{1}{2}", areaNumber, rdType, seqNumber);
            return string.Format("{0}{1}", tmp, this.GetCheckBit(tmp));
        }
        /// <summary>
        /// 验证行政区划
        /// </summary>
        /// <param name="code"></param>
        /// <param name="validLimit"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidArea(string code, AreaValidLimit? validLimit, RegistrationNo15ValidationResult result)
        {
            var areaNumber = this.GetAreaNumber(code);
            var area = AreaHelper.GetDeepestArea(result.AreaNumber, this.AdministrationDictionary);
            result.AreaNumber = areaNumber;
            var valid = true;
            if (area != null)
            {
                result.RecognizableArea = area;
            }
            else
            {
                valid = base.ValidArea(code, validLimit, result);
            }
            return valid;
        }
        /// <summary>
        /// 获取行政区划代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override int GetAreaNumber(string code)
        {
            return int.Parse(code.Substring(0, 6));
        }
        /// <summary>
        /// 验证校验位
        /// </summary>
        /// <param name="code"></param>
        /// <param name="rightBit"></param>
        /// <returns></returns>
        protected override bool IsCheckBitRight(string code, out char rightBit)
        {
            rightBit = this.GetCheckBit(code);
            return code[code.Length - 1] == rightBit;
        }
        private char GetCheckBit(string code)
        {
            var mod = GBT17710_1999.MOD_11_10(code.Select(c => (int)c - 48).Take(14).ToArray());
            return (char)(mod + 48);
        }
        /// <summary>
        /// 验证其它信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidOtherInfo(string code, RegistrationNo15ValidationResult result)
        {
            result.SequenceNumber = int.Parse(code.Substring(6, 8));
            return base.ValidOtherInfo(code, result);
        }
    }
}
