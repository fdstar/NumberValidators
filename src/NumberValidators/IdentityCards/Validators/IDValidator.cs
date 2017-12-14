using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.IdentityCards.Validators
{
    public abstract class IDValidator : BaseValidatorWithDictionary<IDValidationResult, int, string>, IIDValidator
    {
        #region props
        protected override IValidationDictionary<int, string> DefaultDictionary => GBT2260_2013.Singleton;
        public abstract IDLength IDLength { get; }
        protected override string EmptyErrorMessage => ErrorMessage.Empty;
        protected override string RegexMatchFailMessage => ErrorMessage.Error;
        #endregion

        #region interface
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
        public virtual string GenerateID(int areaNumber, DateTime birthDay, int sequenceNumber)
        {
            if (areaNumber < 10 || areaNumber > 999999 || sequenceNumber < 0 || sequenceNumber > 999)
            {
                throw new ArgumentException("argument error");
            }
            return this.GenerateID(areaNumber.ToString().PadRight(6, '0'), birthDay, sequenceNumber.ToString().PadLeft(3, '0'));
        }
        protected abstract string GenerateID(string areaNumber, DateTime birthDay, string sequenceNumber);
        public override IDValidationResult Validate(string number)
        {
            return this.Validate(number, ushort.MinValue);
        }

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
        protected virtual bool ValidArea(string idNumber, AreaValidLimit validLimit, IDValidationResult result)
        {
            int areaNumber = this.GetAreaNumber(idNumber);
            var area = this.GetDeepestArea(areaNumber);
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
        /// <summary>
        /// 根据行政编号获取最深的行政区划信息（可根据Parent获取其父级区划）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private Area GetDeepestArea(int code)
        {
            //按GBT2260 - 2007版本说明，已撤销移除的区域编号不会被其它地方使用，那么完全可以只需要GBT2260基类，然后将已移除的行政区划加入Dictonary即可
            //否则的话，可以每期GBT2260标准都如现在一样，设置对应的类，然后按出生日期确定算法从这些集合中找到对应的区域编号
            Area area = null;
            Area lastArea = null;
            var dic = this.Dictionary.GetDictionary();
            while (code > 0)
            {
                //在这里才做if判断是为了防止因为GBT2260标准变化导致的区域id无法获取的情况
                if (dic.ContainsKey(code))
                {
                    var tmpArea = new Area(code, dic[code]);
                    if (area == null)
                    {
                        area = tmpArea;
                    }
                    else
                    {
                        lastArea.Parent = tmpArea;
                    }
                    lastArea = tmpArea;
                }
                code /= 100;
            }
            return area;
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
        /// 获取校验位
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        internal protected abstract bool IsCheckBitRight(string idNumber, out char rightBit);
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
        /// <param name="result"></param>
        protected abstract string GetSequenceNumber(string idNumber);

        #endregion
    }
}
