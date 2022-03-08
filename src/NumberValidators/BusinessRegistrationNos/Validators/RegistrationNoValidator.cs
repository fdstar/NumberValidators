using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// 工商注册号/统一社会信用代码模板基类
    /// </summary>
    public abstract class RegistrationNoValidator<TResult> : BaseValidatorWithDictionary<TResult, int, string>, IRegistrationNoValidator<TResult>
        where TResult : RegistrationNoValidationResult, new()
    {
        #region props
        /// <summary>
        /// 号码长度
        /// </summary>
        public abstract RegistrationNoLength RegistrationNoLength { get; }
        /// <summary>
        /// 默认的行政区划字典
        /// </summary>
        protected override IValidationDictionary<int, string> DefaultDictionary => GBT2260_2013.Singleton;
        /// <summary>
        /// 空提示
        /// </summary>
        protected override string EmptyErrorMessage => ErrorMessage.Empty;
        /// <summary>
        /// 正则验证失败提示
        /// </summary>
        protected override string RegexMatchFailMessage => ErrorMessage.Error;
        #endregion

        #region interfaces
        /// <summary>
        /// 生成随机的号码
        /// </summary>
        /// <returns></returns>
        public override string GenerateRandomNumber()
        {
            var areaNumber = this.Dictionary.GetDictionary().Keys.OrderBy(g => Guid.NewGuid()).FirstOrDefault(i => i > 10000 && i < 1000000);
            if (areaNumber == 0)
            {
                throw new ArgumentException("GB2312 dictionary is not correct.");
            }
            return this.GenerateRegistrationNo(areaNumber.ToString().PadRight(6, '0'));
        }
        /// <summary>
        /// 生成工商注册号/统一社会信用代码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <returns></returns>
        protected abstract string GenerateRegistrationNo(string areaNumber);
        /// <summary>
        /// 验证号码是否正确
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public override TResult Validate(string number)
        {
            return this.Validate(number, null);
        }
        /// <summary>
        /// 验证号码是否正确
        /// </summary>
        /// <param name="code">待验证的工商注册码/统一社会信用代码</param>
        /// <param name="validLimit">行政区划验证限制，因为存在工商管理机构代码，所以默认为null</param>
        public TResult Validate(string code, AreaValidLimit? validLimit = null)
        {
            var result = base.Validate(code);
            _ = result.IsValid
                && this.ValidArea(code, validLimit, result)
                && this.ValidCheckBit(code, result)
                && this.ValidOtherInfo(code, result);
            return result;
        }
        #endregion

        #region methods
        /// <summary>
        /// 验证行政区划
        /// </summary>
        /// <param name="code"></param>
        /// <param name="validLimit"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidArea(string code, AreaValidLimit? validLimit, TResult result)
        {
            var areaNumber = this.GetAreaNumber(code);
            var area = AreaHelper.GetDeepestArea(areaNumber, this.Dictionary);
            bool valid = !validLimit.HasValue || (area != null && area.GetDepth() >= (int)validLimit);
            result.RecognizableArea = area;
            result.AreaNumber = areaNumber;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidArea);
            }
            return valid;
        }
        /// <summary>
        /// 获取行政区划代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected abstract int GetAreaNumber(string code);
        private bool ValidCheckBit(string code, TResult result)
        {
            bool valid = this.IsCheckBitRight(code, out char rightBit);
            result.CheckBit = rightBit;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidCheckBit);
            }
            return valid;
        }
        /// <summary>
        /// 判断校验位是否正确
        /// </summary>
        /// <param name="code">待校验的号码</param>
        /// <param name="rightBit">正确的校验位</param>
        /// <returns></returns>
        protected abstract bool IsCheckBitRight(string code, out char rightBit);
        /// <summary>
        /// 验证其它信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidOtherInfo(string code, TResult result)
        {
            result.RegistrationNoLength = this.RegistrationNoLength;
            return true;
        }
        #endregion
    }
}
