using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices
{
    public class GBT2260OnlyProvince : IValidationDictionary<int, string>
    {
        private GBT2260OnlyProvince() { }
        public static readonly GBT2260OnlyProvince Singleton = new GBT2260OnlyProvince();

        static readonly Dictionary<int, string> Dictionary = new Dictionary<int, string>
        {
            #region 区域代码
            { 1100,"北京市"},
            { 1200,"天津市"},
            { 1300,"河北省"},
            { 1400,"山西省"},
            { 1500,"内蒙古自治区"},
            { 2100,"辽宁省"},
            { 2200,"吉林省"},
            { 2300,"黑龙江省"},
            { 3100,"上海市"},
            { 3200,"江苏省"},
            { 3300,"浙江省"},
            { 3400,"安徽省"},
            { 3500,"福建省"},
            { 3600,"江西省"},
            { 3700,"山东省"},
            { 4100,"河南省"},
            { 4200,"湖北省"},
            { 4300,"湖南省"},
            { 4400,"广东省"},
            { 4500,"广西壮族自治区"},
            { 4600,"海南省"},
            { 5000,"重庆市"},
            { 5100,"四川省"},
            { 5200,"贵州省"},
            { 5300,"云南省"},
            { 5400,"西藏自治区"},
            { 6100,"陕西省"},
            { 6200,"甘肃省"},
            { 6300,"青海省"},
            { 6400,"宁夏回族自治区"},
            { 6500,"新疆维吾尔自治区"},
            { 7100,"台湾省"},
            { 8100,"香港特别行政区"},
            { 8200,"澳门特别行政区"},
            #endregion
        };
        public IDictionary<int, string> GetDictionary()
        {
            return Dictionary;
        }
    }
}
