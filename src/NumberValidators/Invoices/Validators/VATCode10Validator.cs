using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税普通/专用发票代码验证（营改增之后的规则）
    /// </summary>
    public class VATCode10Validator : VATCodeValidator<VATCode10ValidationResult>
    {
        /*1-4区域编码
         * 5-6年份 
         * 7印制批次 
         * 8发票类别（3/6 增值税普通发票，1/5 增值税专用发票，7/2 货物运输业增值税专用发票） 
         * 9联次（发票共有几联） 
         * 10金额版本号(1：万元版,2：十万元版,3：百万元版，4：千万元版,0：电脑发票)*/
        private static readonly SortedDictionary<char, VATKind> _kindDic = new SortedDictionary<char, VATKind>
        {
            {'3',VATKind.Plain },
            {'6',VATKind.Plain},
            {'1',VATKind.Special},
            {'5',VATKind.Special},
            {'2',VATKind.Transport},
            {'7',VATKind.Transport},
        };
        private static readonly Dictionary<VATKind, int> _duplicateDic = new Dictionary<VATKind, int>
        {
            {VATKind.Plain ,2},
            {VATKind.Special ,3},
            {VATKind.Transport ,3},
        };
        /// <summary>
        /// 支持的增值税发票类型
        /// </summary>
        protected override IEnumerable<VATKind> SupportKind => _duplicateDic.Keys;
        /// <summary>
        /// 验证用的正则
        /// </summary>
        protected override string RegexPattern => RegexPatterns.VATCode10;
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
            var rdKind = query.First().Key;
            return string.Format("{0}{1}{2}{3}{4}0", areaNumber, year, (batch % 10).ToString(), rdKind, _duplicateDic[kind]);
        }
        /// <summary>
        /// 获取年份
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected override int GetYear(string vatCode)
        {
            return int.Parse(vatCode.Substring(4, 2)) + 2000;
        }
        /// <summary>
        /// 获取行政区划
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected override int GetAreaNumber(string vatCode)
        {
            return int.Parse(vatCode.Substring(0, 4));
        }
        /// <summary>
        /// 验证类型是否符合
        /// </summary>
        /// <param name="vatCode"></param>
        /// <param name="kind"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidVATKind(string vatCode, VATKind? kind, VATCode10ValidationResult result)
        {
            var valid = _kindDic.ContainsKey(vatCode[7]) && (!kind.HasValue || kind.Value == _kindDic[vatCode[7]]);
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidKind);
            }
            else
            {
                result.Category = _kindDic[vatCode[7]];
            }
            return valid;
        }
        /// <summary>
        /// 验证填充额外信息
        /// </summary>
        /// <param name="vatCode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override bool ValidOtherInfo(string vatCode, VATCode10ValidationResult result)
        {
            result.DuplicateNumber = int.Parse(vatCode.Substring(8, 1));
            result.AmountVersion = (AmountVersion)int.Parse(vatCode.Substring(9, 1));
            return base.ValidOtherInfo(vatCode, result);
        }
        /// <summary>
        /// 获取批次
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected override int GetBatch(string vatCode)
        {
            return int.Parse(vatCode.Substring(7, 1));
        }
    }
}
