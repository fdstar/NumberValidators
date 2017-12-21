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
        /// 无限制，即不校验，适用于特殊情况，注意并非设置不验证，约定实际以默认设置的最低验证级别为允许的最低级别
        /// </summary>
        NoValidLimit = 0,
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
