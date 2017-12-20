using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.Utils
{
    /// <summary>
    /// 行政区划帮助类
    /// </summary>
    public static class AreaHelper
    {
        /// <summary>
        /// 根据行政编号获取最深的行政区划信息（可根据Parent获取其父级区划），注意其转化为字符串长度必须为偶数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Area GetDeepestArea(int code, IValidationDictionary<int, string> dictionary)
        {
            //按GBT2260 - 2007版本说明，已撤销移除的区域编号不会被其它地方使用，那么完全可以只需要GBT2260基类，然后将已移除的行政区划加入Dictonary即可
            //否则的话，可以每期GBT2260标准都如现在一样，设置对应的类，然后按出生日期确定算法从这些集合中找到对应的区域编号
            Area area = null;
            Area lastArea = null;
            var dic = dictionary.GetDictionary();
            while (code > 0)
            {
                if (code < 10)
                {
                    throw new ArgumentException("行政区划代码错误");
                }
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
    }
}
