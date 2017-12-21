using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices.Validators
{
    /// <summary>
    /// 增值税发票代码验证接口
    /// </summary>
    public interface IVATCodeValidator <out TResult>: IValidator<TResult>
        where TResult : VATCodeValidationResult, new()
    {
        /// <summary>
        /// 用于验证的字典数据
        /// </summary>
        IValidationDictionary<int, string> Dictionary { get; set; }
        /// <summary>
        /// 生成增值税发票代码
        /// </summary>
        /// <param name="areaNumber">行政区划</param>
        /// <param name="year">年份</param>
        /// <param name="batch">批次</param>
        /// <param name="kind">要生成的发票类型</param>
        /// <returns></returns>
        string GenerateVATCode(int areaNumber, ushort year, ushort batch, VATKind kind);
        /// <summary>
        /// 发票代码验证
        /// </summary>
        /// <param name="vatCode">待验证的发票代码</param>
        /// <param name="kind">要验证的发票类型，不指定则传null</param>
        /// <param name="minYear">允许的最小年份（注：2012年1月1日营改增开始上海试点）</param>
        /// <returns></returns>
        TResult Validate(string vatCode, VATKind? kind = null, ushort minYear = 2012);
    }
}
