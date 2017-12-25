using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using NumberValidators.Utils.ISO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// GB 32100-2015 法人和其他组织统一社会信用代码编码规则
    /// </summary>
    public class RegistrationNo18Validator : RegistrationNoValidator<RegistrationNo18ValidationResult>
    {
        #region fields
        /// <summary>
        /// 本体代码及其对应的代码字符数值,不使用字母IOZSV
        /// </summary>
        internal static readonly Dictionary<char, int> CodeDictionary = new Dictionary<char, int>();
        /// <summary>
        /// 加权因子
        /// </summary>
        internal static readonly int[] WeightingFactors = { 1, 3, 9, 27, 19, 26, 16, 17, 20, 29, 25, 13, 8, 24, 10, 30, 28 };
        private static readonly IEnumerable<ManagementCode> ManagementCodes = Enum.GetValues(typeof(ManagementCode)).Cast<ManagementCode>();
        private static readonly IEnumerable<int> ManagementKindCodes = Enum.GetValues(typeof(ManagementKindCode)).Cast<ManagementKindCode>().Select(c => (int)c);
        #endregion

        #region props
        /// <summary>
        /// 号码长度
        /// </summary>
        public override RegistrationNoLength RegistrationNoLength => RegistrationNoLength.Eighteen;
        /// <summary>
        /// 验证用的正则
        /// </summary>
        protected override string RegexPattern => @"^[0-9A-HJ-NP-RTUW-Y]{2}\d{6}[0-9A-HJ-NP-RTUW-Y]{10}$";
        #endregion

        #region oct
        static RegistrationNo18Validator()
        {
            for (var i = 0; i <= 30; i++)
            {
                int num;
                if (i > 27) { num = 59; }
                else if (i > 25) { num = 58; }
                else if (i > 22) { num = 57; }
                else if (i > 17) { num = 56; }
                else if (i > 9) { num = 55; }
                else { num = 48; }
                var c = (char)(num + i);
                CodeDictionary.Add(c, i);
            }
        }
        #endregion
        /// <summary>
        /// 生成随机的统一社会信用代码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <returns></returns>
        protected override string GenerateRegistrationNo(string areaNumber)
        {
            ManagementCode code = ManagementCodes.OrderBy(g => Guid.NewGuid()).First();
            ManagementKindCode kind = ManagementKindCode.NonSpecific;
            var query = ManagementKindCodes.Where(c => c / 100 == (int)code);
            if (query.Any())
            {
                kind = (ManagementKindCode)query.OrderBy(g => Guid.NewGuid()).First();
            }
            return this.GenerateRegistrationNo(areaNumber, code, kind);
        }
        /// <summary>
        /// 生成统一社会信用代码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="code"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public string GenerateRegistrationNo(int areaNumber, ManagementCode code, ManagementKindCode kind)
        {
            return this.GenerateRegistrationNo(areaNumber.ToString().PadRight(6, '0').Substring(0, 6), code, kind);
        }
        private string GenerateRegistrationNo(string areaNumber, ManagementCode code, ManagementKindCode kind)
        {
            var tmp = string.Format("{0}{1}{2}{3}", (char)((int)code), (char)((int)kind % 100), areaNumber, this.GenerateOrganizationCode());
            return string.Format("{0}{1}", tmp, this.GetCheckBit(tmp));
        }
        private string GenerateOrganizationCode()
        {
            var tmp = new string(Enumerable.Repeat(1, 4).SelectMany(i =>
            {
                var rd = RandomHelper.GetRandomNumber(961);
                return new int[] { rd / 31, rd % 31 };
            }).Select(k => CodeDictionary.First(kv => kv.Value == k).Key).ToArray());
            return string.Format("{0}{1}", tmp, this.GetOrganizationCodeCheckBit(tmp));
        }
        /// <summary>
        /// 获取行政区划编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override int GetAreaNumber(string code)
        {
            return int.Parse(code.Substring(2, 6));
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
            return rightBit == code[17];
        }
        private char GetCheckBit(string code)
        {
            var mod = (31 - ISO7064_1983.MOD_11_2(code.Select(c => CodeDictionary[c]).Take(17).ToArray(), WeightingFactors, 31)) % 31;
            return CodeDictionary.First(kv => kv.Value == mod).Key;
        }
        /// <summary>
        /// 验证其它信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidOtherInfo(string code, RegistrationNo18ValidationResult result)
        {
            return base.ValidOtherInfo(code, result)
                && this.ValidManagementCode(code, result)
                && this.ValidManagementKindCode(code, result)
                && this.ValidOrganizationCode(code, result);
        }
        private bool ValidManagementCode(string code, RegistrationNo18ValidationResult result)
        {
            var valid = Enum.TryParse(((int)code[0]).ToString(), out ManagementCode mc);
            result.ManagementCode = mc;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidManagement);
            }
            return valid;
        }
        private bool ValidManagementKindCode(string code, RegistrationNo18ValidationResult result)
        {
            var valid = Enum.TryParse(((int)result.ManagementCode * 100 + (int)code[1]).ToString(), out ManagementKindCode mkc)
                || (int)code[1] == (int)ManagementKindCode.NonSpecific;
            result.ManagementKindCode = mkc == 0 ? ManagementKindCode.NonSpecific : mkc;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidManagementKind);
            }
            return valid;
        }
        private bool ValidOrganizationCode(string code, RegistrationNo18ValidationResult result)
        {
            var orgCode = code.Substring(8, 9);
            result.OrganizationCode = orgCode;
            var valid = orgCode[8] == this.GetOrganizationCodeCheckBit(orgCode);
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidOrganizationCode);
            }
            return valid;
        }
        private char GetOrganizationCodeCheckBit(string orgCode)
        {
            return GBT11714_1997.GetCheckBit(orgCode);
        }
    }
}
