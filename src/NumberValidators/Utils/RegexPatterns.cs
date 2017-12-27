using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Utils
{
    /// <summary>
    /// 证件相关的正则表达式
    /// </summary>
    public static class RegexPatterns
    {
        /*
         * 港澳台通行证参考地址 http://www.xm-l-tax.gov.cn/xmdscms/fgk/news/20040.html
         */

        /// <summary>
        /// 台湾居民来往大陆通行证（包含旧规则）
        /// 因为2006年5月开始，内地公安机关签发的5年台胞通行证取消了小括号（即原办证机构代码），而目前已经是2017，所以已不存在有效期内有小括号的通行证，故正则不考虑小括号
        /// 理论上应该也不再存在有效期内的10位台湾居民来往大陆通行证，但这里还做支持
        /// </summary>
        public const string TaiwanPassportFull = @"^\d{8}(\d{2})?$";
        /// <summary>
        /// 台湾居民来往大陆通行证（最新规则）
        /// </summary>
        public const string TaiwanPassport = @"^\d{8}$";
        /// <summary>
        /// 港澳居民来往内地通行证（包含旧规则）
        /// </summary>
        public const string HongKongAndMacaoPassportFull = @"^[HM]\d{8}(\d{2})?$";
        /// <summary>
        /// 港澳居民来往内地通行证（最新规则）
        /// </summary>
        public const string HongKongAndMacaoPassport = @"^[HM]\d{8}$";
        /// <summary>
        /// 港澳台通行证（包含旧规则）
        /// </summary>
        public const string HMTPassportFull = @"^[HM]?\d{8}(\d{2})?$";
        /// <summary>
        /// 港澳台通行证（最新规则）
        /// </summary>
        public const string HMTPassport = @"^[HM]?\d{8}$";
        /// <summary>
        /// 15位一代大陆居民身份证
        /// </summary>
        public const string FirstIdentityCard = @"^[1-8]\d{14}$";
        /// <summary>
        /// 18位二代大陆居民身份证
        /// </summary>
        public const string SecondIdentityCard = @"^[1-8]\d{16}[0-9xX]$";
        /// <summary>
        /// 大陆居民身份证
        /// </summary>
        public const string IdentityCard = @"^[1-8]\d{14}(\d{2}[0-9xX])?$";
        /// <summary>
        /// 增值税普通、专用发票代码、货物运输业增值税专用发票
        /// </summary>
        public const string VATCode10 = @"^\d{8}[2-9][0-4]$";
        /// <summary>
        /// 增值税电子普通、普通（卷票）
        /// </summary>
        public const string VATCode12 = @"^0\d{11}$";
        /// <summary>
        /// 工商注册码
        /// </summary>
        public const string BusinessRegistrationNo = @"^\d{15}$";
        /// <summary>
        /// 法人和其他组织统一社会信用代码
        /// </summary>
        public const string UnifiedSocialCreditCode = @"^[0-9A-HJ-NP-RTUW-Y]{2}\d{6}[0-9A-HJ-NP-RTUW-Y]{10}$";
    }
}
