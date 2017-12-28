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
        public const string TaiwanPassFull = @"^\d{8}(\d{2})?$";
        /// <summary>
        /// 台湾居民来往大陆通行证（最新规则）
        /// </summary>
        public const string TaiwanPass = @"^\d{8}$";
        /// <summary>
        /// 港澳居民来往内地通行证（包含旧规则）
        /// </summary>
        public const string HongKongAndMacaoPassFull = @"^[HM]\d{8}(\d{2})?$";
        /// <summary>
        /// 港澳居民来往内地通行证（最新规则）
        /// </summary>
        public const string HongKongAndMacaoPass = @"^[HM]\d{8}$";
        /// <summary>
        /// 港澳台通行证（包含旧规则）
        /// </summary>
        public const string HMTPassFull = @"^[HM]?\d{8}(\d{2})?$";
        /// <summary>
        /// 港澳台通行证（最新规则）
        /// </summary>
        public const string HMTPass = @"^[HM]?\d{8}$";
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
        /// <summary>
        /// 中国大陆护照，注意不包含特区护照（港澳特区护照）
        /// 因私电子护照新字段2017.04启动 http://www.ailvxing.com/info-103-24211-0.html 第二位使用字母，排除IO，暂时这里符合规定只允许第二位输入字母，估计未来后7位也可能采用同样规则
        /// 按知乎上的回复，2017年8月底已经是EB开头的护照，大致估算是4个月用掉一个字段，24个字段大致能用96个月，也就是8年，即至少到2025年应该还不会采用第三个字段英文字母
        /// 因公电子护照规则 http://dfoca.hainan.gov.cn/wsqbzw/gzdt/201205/t20120507_671172.html 2012开始启用因公电子护照，对应有效期，目前应该不再存在非电子因公护照了，但这里匹配上还做支持
        /// </summary>
        public const string ChinesePassport = @"^(?:G\d{8}|[DSP]E?\d{7}|E[0-9A-HJ-NP-Z]\d{7})$";
        /// <summary>
        /// 通用护照标准，按国际民航组织9303约定，最大不能超过9位
        /// </summary>
        public const string GeneralPassport = @"^[0-9A-Za-z]{5,9}$";

        /*
         * 纳税人识别号 http://blog.sina.com.cn/s/blog_5fb8de860102vvpd.html
         * 组织结构是15位，法人是20位，补全依据 http://www.doc88.com/p-3985541443573.html
         * 三证合一后的社会统一验证代码也是纳税人识别号
         * 自然人是18位，对于一代身份证不清楚是否和法人一样补全3位0
         * 其它证件，也就是15位的无法获取校验位计算规则
         */
    }
}
