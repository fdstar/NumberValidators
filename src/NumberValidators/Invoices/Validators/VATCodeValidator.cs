using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    public abstract class VATCodeValidator : BaseValidatorWithDictionary<VATCodeValidationResult, int, string>, IVATCodeValidator
    {
        #region props
        protected override IValidationDictionary<int, string> DefaultDictionary => GBT2260OnlyProvince.Singleton;
        protected abstract IEnumerable<VATKind> SupportKind { get; }
        protected override string EmptyErrorMessage => ErrorMessage.Empty;
        protected override string RegexMatchFailMessage => ErrorMessage.Error;
        #endregion

        #region interface
        public override string GenerateRandomNumber()
        {
            var areaNumber = this.Dictionary.GetDictionary().Keys.OrderBy(g => Guid.NewGuid()).FirstOrDefault(i => i >= 10 && i < 10000);
            if (areaNumber == 0)
            {
                throw new ArgumentException("GB2312 dictionary is not correct.");
            }
            var yearNow = DateTime.Now.Year;
            var year = yearNow - RandomHelper.GetRandomNumber(yearNow - 2012);
            var batch = RandomHelper.GetRandomNumber(1000);
            return this.GenerateVATCode(areaNumber, (ushort)year, (ushort)batch, SupportKind.OrderBy(g => Guid.NewGuid()).First());
        }
        public virtual string GenerateVATCode(int areaNumber, ushort year, ushort batch, VATKind kind)
        {
            if (areaNumber < 10 || areaNumber > 9999)
            {
                throw new ArgumentException("argument error");
            }
            return this.GenerateVATCode(areaNumber.ToString().PadRight(4, '0'), (year % 100).ToString().PadLeft(2, '0'), batch, kind);
        }
        protected abstract string GenerateVATCode(string areaNumber, string year, ushort batch, VATKind kind);

        public override VATCodeValidationResult Validate(string number)
        {
            return this.Validate(number, minYear: 2012);
        }

        public VATCodeValidationResult Validate(string vatCode, VATKind? kind = null, ushort minYear = 2012)
        {
            var result = base.Validate(vatCode);
            var valid = result.IsValid
                && this.ValidYear(vatCode, minYear, result)
                && this.ValidArea(vatCode, result)
                && this.ValidVATKind(vatCode, kind, result)
                && this.ValidOtherInfo(vatCode, result);
            return result;
        }
        #endregion

        #region methods
        private bool ValidYear(string vatCode, ushort minYear, VATCodeValidationResult result)
        {
            var year = this.GetYear(vatCode);
            var yearNow = DateTime.Now.Year;
            var valid = minYear >= year && year <= yearNow;
            result.Year = year;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.YearOutOfRange, minYear, yearNow);
            }
            return valid;
        }
        /// <summary>
        /// 获取发票年份
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected abstract int GetYear(string vatCode);
        private bool ValidArea(string vatCode, VATCodeValidationResult result)
        {
            var areaNumber = this.GetAreaNumber(vatCode);
            var dic = this.Dictionary.GetDictionary();
            var valid = dic.ContainsKey(areaNumber);
            result.AreaNumber = areaNumber;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidArea);
            }
            else
            {
                result.AreaName = dic[areaNumber];
            }
            return true;
        }
        /// <summary>
        /// 获取行政区划代码
        /// </summary>
        /// <param name="vatCode"></param>
        /// <returns></returns>
        protected abstract int GetAreaNumber(string vatCode);
        /// <summary>
        /// 验证发票类型是否正确
        /// </summary>
        /// <param name="vatCode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected abstract bool ValidVATKind(string vatCode, VATKind? kind, VATCodeValidationResult result);
        protected virtual bool ValidOtherInfo(string vatCode, VATCodeValidationResult result)
        {
            result.Batch = this.GetBatch(vatCode);
            return true;
        }
        /// <summary>
        /// 获取发票批次
        /// </summary>
        protected abstract int GetBatch(string vatCode);
        #endregion
    }
}
