using NumberValidators.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税发票验证类
    /// </summary>
    public abstract class VATCodeValidator<TResult> : BaseValidatorWithDictionary<TResult, int, string>, IVATCodeValidator<TResult>
        where TResult : VATCodeValidationResult, new()
    {
        #region props
        /// <summary>
        /// 默认基础数据字典
        /// </summary>
        protected override IValidationDictionary<int, string> DefaultDictionary => GBT2260OnlyProvince.Singleton;
        /// <summary>
        /// 支持的增值税发票类型
        /// </summary>
        protected abstract IEnumerable<VATKind> SupportKind { get; }
        /// <summary>
        /// 空错误提示
        /// </summary>
        protected override string EmptyErrorMessage => ErrorMessage.Empty;
        /// <summary>
        /// 正则验证失败错误提示
        /// </summary>
        protected override string RegexMatchFailMessage => ErrorMessage.Error;
        #endregion

        #region interface
        /// <summary>
        /// 生成随机发票代码
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 按输入生成发票代码
        /// </summary>
        /// <param name="areaNumber">行政区划代码</param>
        /// <param name="year">发票年份</param>
        /// <param name="batch">批次</param>
        /// <param name="kind">发票类型</param>
        /// <returns></returns>
        public virtual string GenerateVATCode(int areaNumber, ushort year, ushort batch, VATKind kind)
        {
            if (areaNumber < 10 || areaNumber > 9999)
            {
                throw new ArgumentException("argument error");
            }
            return this.GenerateVATCode(areaNumber.ToString().PadRight(4, '0'), (year % 100).ToString().PadLeft(2, '0'), batch, kind);
        }
        /// <summary>
        /// 按输入生成发票代码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="year"></param>
        /// <param name="batch"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        protected abstract string GenerateVATCode(string areaNumber, string year, ushort batch, VATKind kind);
        /// <summary>
        /// 验证发票代码是否正确
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public override TResult Validate(string number)
        {
            return this.Validate(number, minYear: 2012);
        }
        /// <summary>
        /// 验证发票代码是否正确
        /// </summary>
        /// <param name="vatCode">发票代码</param>
        /// <param name="kind">发票类型</param>
        /// <param name="minYear">最小年份</param>
        /// <returns></returns>
        public virtual TResult Validate(string vatCode, VATKind? kind = null, ushort minYear = 2012)
        {
            var result = base.Validate(vatCode);
            var valid = result.IsValid
                && this.ValidYear(result.Number, minYear, result)
                && this.ValidArea(result.Number, result)
                && this.ValidVATKind(result.Number, kind, result)
                && this.ValidOtherInfo(result.Number, result);
            return result;
        }
        #endregion

        #region methods
        private bool ValidYear(string vatCode, ushort minYear, TResult result)
        {
            var year = this.GetYear(vatCode);
            var yearNow = DateTime.Now.Year;
            var valid = year >= minYear && year <= yearNow;
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
        private bool ValidArea(string vatCode, TResult result)
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
            return valid;
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
        protected abstract bool ValidVATKind(string vatCode, VATKind? kind, TResult result);
        /// <summary>
        /// 验证填充额外信息
        /// </summary>
        /// <param name="vatCode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidOtherInfo(string vatCode, TResult result)
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
