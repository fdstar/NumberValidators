using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators
{
    /// <summary>
    /// 行政区划
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 行政区划构造函数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
        public Area(int number, string name)
        {
            if (name == null || number.ToString().Length % 2 != 0)
            {
                throw new ArgumentException("The number or name is not correct!");
            }
            this.Number = number;
            this.Name = name.Trim();
        }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        public int Number { get; }
        /// <summary>
        /// 行政区域名称（仅当前节点）
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 行政区域完整名称（包含上级行政区域名称）
        /// </summary>
        public string FullName
        {
            get
            {
                StringBuilder tmp = new StringBuilder();
                var area = this;
                while (area != null)
                {
                    tmp.Insert(0, area.Name);
                    area = area.Parent;
                }
                return tmp.ToString();
            }
        }
        /// <summary>
        /// 父级区域
        /// </summary>
        public Area Parent { get; internal set; }

        /// <summary>
        /// 获取当前区域的深度，从1开始，按照GB2260行政区域编码规则2位为一个深度
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            return this.Number.ToString().Length / 2;
        }
    }
}
