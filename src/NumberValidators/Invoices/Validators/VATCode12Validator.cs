using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税电子/卷票发票代码验证
    /// 电子发票规则参考 http://www.chinatax.gov.cn/n810341/n810755/c1919901/content.html
    /// 卷票规则参考 http://www.chinatax.gov.cn/n810341/n810755/c2420199/content.html
    /// 收费公路通行费增值税电子普通发票规则参考 http://www.chinatax.gov.cn/n810341/n810755/c2985595/content.html
    /// 定额发票虽然是12位，但根据《全面推开营改增试点工作（发票）2016年第8号——关于明确通用定额发票使用有关问题的通知》2017.01.01后只有提供车辆停放服务的纳税人、起征点以下的纳税人才可以继续使用定额发票，所以此处不予支持(https://wenku.baidu.com/view/12d624c2d4d8d15abf234e12.html)
    /// 区块链电子普通发票的发票代码 例如144031809110,第6－7位为年份，第8位0代表行业种类为通用类、第9位9代表深圳电子普通发票专属种类类别，第10位代表批次，第11位代表联次，第12位0代表无限制金额版 http://www.360doc.com/content/18/0905/12/12373774_784061533.shtml
    /// 电子专票的发票代码为12位，编码规则：第1位为0，第2-5位代表省、自治区、直辖市和计划单列市，第6-7位代表年度，第8-10位代表批次，第11-12位为13。发票号码为8位，按年度、分批次编制
    /// </summary>
    public class VATCode12Validator : VATCodeValidator<VATCode12ValidationResult>
    {
        /* 第1位为0或1  1为区块链发票 0为电子发票或卷票
         * 第2-5位代表省、自治区、直辖市和计划单列市
         * 第6-7位代表年度
         * 第8-10位代表批次
         * 第11-12位代表票种和规格/联次
         ** （11代表增值税电子普通发票，
         ** 12代表收费公路通行费增值税电子普通发票，注意通行费发票同专票一样可以进行发票抵扣，但发票查验时还是通过校验码查询，
         *  13代表增值税电子专票
         ** 06代表57mm×177.8mm增值税普通发票（卷票），07代表76mm×177.8mm增值税普通发票（卷票），
         ** 04代表二联增值税普通发票（折叠票），05代表五联增值税普通发票（折叠票））
         * 发票号码为8位，按年度、分批次编制。*/
        private static readonly Dictionary<string, VATKind> _kindDic = new Dictionary<string, VATKind>
        {
            {"11",VATKind.Electronic },
            {"12",VATKind.Electronic },
            {"13",VATKind.Electronic },
            {"06",VATKind.Roll},
            {"07",VATKind.Roll},
            {"04",VATKind.Plain},
            {"05",VATKind.Plain},
        };
        /// <summary>
        /// 支持的增值税发票类型
        /// </summary>
        protected override IEnumerable<VATKind> SupportKind => _kindDic.Values.Distinct();
        /// <summary>
        /// 验证用的正则
        /// </summary>
        protected override string RegexPattern => RegexPatterns.VATCode12;
        /// <summary>
        /// 生成增值税发票代码（注意不支持生成区块链发票）
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="year"></param>
        /// <param name="batch"></param>
        /// <param name="kind"></param>
        /// <param name="electKind">如果发票种类为电子发票时，要生成的电子发票细分类型</param>
        /// <returns></returns>
        protected override string GenerateVATCode(string areaNumber, string year, ushort batch, VATKind kind, ElectronicVATKind? electKind)
        {
            var query = _kindDic.Where(kv => kv.Value == kind);
            if (!query.Any())
            {
                throw new NotSupportedException(ErrorMessage.GenerateWrongKind);
            }
            var rdKind = query.OrderBy(g => Guid.NewGuid()).First().Key;
            if (electKind.HasValue)
            {
                rdKind = Convert.ToInt32(electKind).ToString();
            }
            return string.Format("0{0}{1}{2}{3}", areaNumber, year, (batch % 1000).ToString().PadLeft(3, '0'), rdKind);
        }
        /// <summary>
        /// 获取年份
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected override int GetYear(string vatCode)
        {
            return int.Parse(vatCode.Substring(5, 2)) + 2000;
        }
        /// <summary>
        /// 获取行政区划
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected override int GetAreaNumber(string vatCode)
        {
            return int.Parse(vatCode.Substring(1, 4));
        }
        /// <summary>
        /// 区域校验时是否忽略34两位行政区划编码
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        protected override bool AreaValidSkip34(VATKind kind)
        {
            if (kind == VATKind.Electronic)
            {//电子发票行政区划可能会具体到市级，吉林、安徽、山东、湖北 都发现具体到市级行政编号的情况，所以电子发票支持只查前2位行政区划
                return true;
            }
            return base.AreaValidSkip34(kind);
        }
        /// <summary>
        /// 验证类型是否符合
        /// </summary>
        /// <param name="vatCode"></param>
        /// <param name="kind"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidVATKind(string vatCode, VATKind? kind, VATCode12ValidationResult result)
        {
            if (vatCode[0] == '1')
            {
                //区块链发票以1开头
                return this.ValidVATKindWithBlockchain(vatCode, kind, result);
            }
            else
            {
                //增值税发票以0开头
                return this.ValidVATKindWithoutBlockchain(vatCode, kind, result);
            }
        }
        private bool ValidVATKindWithBlockchain(string vatCode, VATKind? kind, VATCode12ValidationResult result)
        {
            //区块链发票以1开头
            if (vatCode[11] == '0' && (!kind.HasValue || kind.Value == VATKind.Blockchain))
            {
                result.Category = VATKind.Blockchain;
                result.DuplicateNumber = int.Parse(vatCode[10].ToString());
                return true;
            }
            result.AddErrorMessage(ErrorMessage.InvalidKind);
            return false;
        }
        private bool ValidVATKindWithoutBlockchain(string vatCode, VATKind? kind, VATCode12ValidationResult result)
        {
            //增值税发票以0开头
            var key = vatCode.Substring(10, 2);
            var valid = _kindDic.ContainsKey(key) && (!kind.HasValue || kind.Value == _kindDic[key]);
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidKind);
            }
            else
            {
                result.Category = _kindDic[key];
                switch (result.Category)
                {
                    case VATKind.Plain:
                        result.DuplicateNumber = key == "04" ? 2 : 5;
                        break;
                    case VATKind.Electronic:
                        if (Enum.TryParse(key, out ElectronicVATKind eKind))
                        {
                            result.ElectronicVATKind = eKind;
                        }
                        break;
                    default: break;
                }
            }
            return valid;
        }
        /// <summary>
        /// 获取批次
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected override int GetBatch(string vatCode)
        {
            if (vatCode[0] == '0')
            {
                //增值税发票
                return int.Parse(vatCode.Substring(7, 3));
            }
            else
            {
                //区块链电子发票
                return int.Parse(vatCode[9].ToString());
            }
        }
    }
}
