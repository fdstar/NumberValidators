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
    /// 定额发票虽然是12位，但根据《全面推开营改增试点工作（发票）2016年第8号——关于明确通用定额发票使用有关问题的通知》2017.01.01后只有提供车辆停放服务的纳税人、起征点以下的纳税人才可以继续使用定额发票，所以此处不予支持(https://wenku.baidu.com/view/12d624c2d4d8d15abf234e12.html)
    /// </summary>
    public class VATCode12Validator : VATCodeValidator<VATCodeValidationResult>
    {
        /* 第1位为0
         * 第2-5位代表省、自治区、直辖市和计划单列市
         * 第6-7位代表年度
         * 第8-10位代表批次
         * 第11-12位代表票种和规格（11代表增值税电子普通发票，06代表57mm×177.8mm增值税普通发票（卷票），07代表76mm×177.8mm增值税普通发票（卷票））
         * 发票号码为8位，按年度、分批次编制。*/
        private static readonly Dictionary<string, VATKind> _kindDic = new Dictionary<string, VATKind>
        {
            {"11",VATKind.Electronic },
            {"06",VATKind.Roll},
            {"07",VATKind.Roll},
        };
        /// <summary>
        /// 支持的增值税发票类型
        /// </summary>
        protected override IEnumerable<VATKind> SupportKind => new VATKind[] { VATKind.Electronic, VATKind.Roll };
        /// <summary>
        /// 验证用的正则
        /// </summary>
        protected override string RegexPattern => RegexPatterns.VATCode12;
        /// <summary>
        /// 生成增值税发票代码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="year"></param>
        /// <param name="batch"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        protected override string GenerateVATCode(string areaNumber, string year, ushort batch, VATKind kind)
        {
            var query = _kindDic.Where(kv => kv.Value == kind);
            if (!query.Any())
            {
                throw new NotSupportedException(ErrorMessage.GenerateWrongKind);
            }
            var rdKind = query.OrderBy(g => Guid.NewGuid()).First().Key;
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
        /// 验证类型是否符合
        /// </summary>
        /// <param name="vatCode"></param>
        /// <param name="kind"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidVATKind(string vatCode, VATKind? kind, VATCodeValidationResult result)
        {
            var key = vatCode.Substring(10, 2);
            var valid = _kindDic.ContainsKey(key) && (!kind.HasValue || kind.Value == _kindDic[key]);
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidKind);
            }
            else
            {
                result.Category = _kindDic[key];
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
            return int.Parse(vatCode.Substring(7, 3));
        }
    }
}
