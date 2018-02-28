using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    /// <summary>
    /// 身份证验证模板基类
    /// </summary>
    public abstract class IDValidator : BaseValidatorWithDictionary<IDValidationResult, int, string>, IIDValidator
    {
        #region props
        /// <summary>
        /// 默认数据字典类
        /// </summary>
        protected override IValidationDictionary<int, string> DefaultDictionary => GBT2260_2013.Singleton;
        /// <summary>
        /// 身份证长度
        /// </summary>
        public abstract IDLength IDLength { get; }
        /// <summary>
        /// 空提示
        /// </summary>
        protected override string EmptyErrorMessage => ErrorMessage.Empty;
        /// <summary>
        /// 正则验证失败提示
        /// </summary>
        protected override string RegexMatchFailMessage => ErrorMessage.Error;
        #endregion

        #region interface
        /// <summary>
        /// 生成随机的身份证号码
        /// </summary>
        /// <returns></returns>
        public override string GenerateRandomNumber()
        {
            var areaNumber = this.Dictionary.GetDictionary().Keys.OrderBy(g => Guid.NewGuid()).FirstOrDefault(i => i > 10000 && i < 1000000);
            if (areaNumber == 0)
            {
                throw new ArgumentException("GB2312 dictionary is not correct.");
            }
            var dtNow = DateTime.Now.Date;
            var year = dtNow.Year - RandomHelper.GetRandomNumber(120);
            var month = RandomHelper.GetRandomNumber(dtNow.Year == year ? dtNow.Month : 12) + 1;
            var tmpDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            var day = RandomHelper.GetRandomNumber((dtNow < tmpDate ? dtNow : tmpDate).Day) + 1;
            var sequenceNumber = RandomHelper.GetRandomNumber(1000);
            return this.GenerateID(areaNumber, new DateTime(year, month, day), sequenceNumber);
        }
        /// <summary>
        /// 生成身份证号码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="birthDay"></param>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        public virtual string GenerateID(int areaNumber, DateTime birthDay, int sequenceNumber)
        {
            if (areaNumber < 10 || areaNumber > 999999 || sequenceNumber < 0 || sequenceNumber > 999)
            {
                throw new ArgumentException("argument error");
            }
            return this.GenerateID(areaNumber.ToString().PadRight(6, '0'), birthDay, sequenceNumber.ToString().PadLeft(3, '0'));
        }
        /// <summary>
        /// 生成身份证号码
        /// </summary>
        /// <param name="areaNumber"></param>
        /// <param name="birthDay"></param>
        /// <param name="sequenceNumber"></param>
        /// <returns></returns>
        protected abstract string GenerateID(string areaNumber, DateTime birthDay, string sequenceNumber);
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public override IDValidationResult Validate(string number)
        {
            return this.Validate(number, ushort.MinValue);
        }
        /// <summary>
        /// 验证身份证是否正确
        /// </summary>
        /// <param name="idNumber">待验证的证件号码</param>
        /// <param name="minYear">允许最小年份，默认0</param>
        /// <param name="validLimit">验证区域级别，默认AreaValidLimit.Province</param>
        /// <param name="ignoreCheckBit">是否忽略校验位验证，默认false</param>
        /// <returns>验证结果</returns>
        public IDValidationResult Validate(string idNumber, ushort minYear = 0, AreaValidLimit validLimit = AreaValidLimit.Province, bool ignoreCheckBit = false)
        {
            var result = base.Validate(idNumber);
            var valid = result.IsValid
                && this.ValidBirthday(idNumber, minYear, result)
                && this.ValidArea(idNumber, validLimit, result)
                && this.ValidCheckBit(idNumber, ignoreCheckBit, result)
                && this.ValidOtherInfo(idNumber, result);
            return result;
        }
        #endregion

        #region methods
        private bool ValidBirthday(string idNumber, ushort minYear, IDValidationResult result)
        {
            var birthday = this.GetBirthday(idNumber);
            var dtNow = DateTime.Now.Date;
            bool valid = birthday > DateTime.MinValue && birthday.Year >= minYear && birthday <= dtNow;
            result.Birthday = birthday;
            if (!valid)
            {
                if (birthday == DateTime.MinValue || birthday > dtNow)
                {
                    result.AddErrorMessage(ErrorMessage.InvalidBirthday);
                }
                else
                {
                    result.AddErrorMessage(ErrorMessage.BirthdayYearOutOfRange, minYear, dtNow.Year);
                }
            }
            return valid;
        }
        /// <summary>
        /// 根据证件号获取出生日期，如果获取失败则返回DateTime.MinValue
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        protected abstract DateTime GetBirthday(string idNumber);
        /// <summary>
        /// 验证行政区划
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="validLimit"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidArea(string idNumber, AreaValidLimit validLimit, IDValidationResult result)
        {
            int areaNumber = this.GetAreaNumber(idNumber);
            var area = AreaHelper.GetDeepestArea(areaNumber, this.Dictionary);
            bool valid = area != null && area.GetDepth() >= (int)validLimit;
            result.RecognizableArea = area;
            result.AreaNumber = areaNumber;
            if (!valid)
            {
                if (area == null)
                {
                    result.AddErrorMessage(ErrorMessage.InvalidArea);
                }
                else
                {
                    result.AddErrorMessage(ErrorMessage.AreaLimitOutOfRange, validLimit);
                }
            }
            return valid;
        }
        /// <summary>
        /// 获取行政区划编号
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        protected virtual int GetAreaNumber(string idNumber)
        {
            return int.Parse(idNumber.Substring(0, 6));
        }
        private bool ValidCheckBit(string idNumber, bool ignoreCheckBit, IDValidationResult result)
        {
            bool valid = this.IsCheckBitRight(idNumber, out char rightBit);
            result.CheckBit = rightBit;
            valid = valid || ignoreCheckBit;
            if (!valid)
            {
                result.AddErrorMessage(ErrorMessage.InvalidCheckBit);
            }
            return valid;
        }
        /// <summary>
        /// 判断校验位是否正确
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="rightBit"></param>
        /// <returns></returns>
        protected abstract bool IsCheckBitRight(string idNumber, out char rightBit);
        /// <summary>
        /// 验证并填充其它信息
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ValidOtherInfo(string idNumber, IDValidationResult result)
        {
            int sequence = int.Parse(this.GetSequenceNumber(idNumber));
            result.Gender = (Gender)(sequence % 2);
            result.Sequence = sequence;
            result.IDLength = (IDLength)idNumber.Length;
            return true;
        }
        /// <summary>
        /// 获取出生登记序列号
        /// </summary>
        /// <param name="idNumber"></param>
        protected abstract string GetSequenceNumber(string idNumber);

        #endregion
    }
}
