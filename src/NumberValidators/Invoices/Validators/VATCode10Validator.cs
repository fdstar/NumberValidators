using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税普通/专用发票代码验证（营改增之后的规则）
    /// </summary>
    public class VATCode10Validator : VATCodeValidator
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
        protected override IEnumerable<VATKind> SupportKind => _duplicateDic.Keys;
        protected override string RegexPattern => @"^\d{8}[2-9][0-4]$";

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
        protected override int GetYear(string vatCode)
        {
            return int.Parse(vatCode.Substring(4, 2)) + 2000;
        }
        protected override int GetAreaNumber(string vatCode)
        {
            return int.Parse(vatCode.Substring(0, 4));
        }
        protected override bool ValidVATKind(string vatCode, VATKind? kind, VATCodeValidationResult result)
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
        protected override bool ValidOtherInfo(string vatCode, VATCodeValidationResult result)
        {
            result.DuplicateNumber = int.Parse(vatCode.Substring(8, 1));
            result.AmountVersion = (AmountVersion)int.Parse(vatCode.Substring(9, 1));
            return base.ValidOtherInfo(vatCode, result);
        }
        protected override int GetBatch(string vatCode)
        {
            return int.Parse(vatCode.Substring(7, 1));
        }
    }
}
