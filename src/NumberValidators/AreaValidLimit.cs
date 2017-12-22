using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators
{
    /// <summary>
    /// 区域验证级别限制
    /// </summary>
    public enum AreaValidLimit
    {
        /// <summary>
        /// 省，即身份证号码的前两位，新中国建立以来行政区域列表还未对省级做过调整
        /// </summary>
        Province = 1,
        /// <summary>
        /// 市，即身份证号码的前四位，新中国建立以来对少数市级区域做过调整
        /// </summary>
        City = 2,
        /// <summary>
        /// 县，即身份证号码的前六位，县级调整是最频繁的
        /// </summary>
        County = 3
    }
}
