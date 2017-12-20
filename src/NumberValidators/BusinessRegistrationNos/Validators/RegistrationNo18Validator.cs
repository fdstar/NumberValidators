using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos.Validators
{
    /// <summary>
    /// GB 32100-2015 法人和其他组织统一社会信用代码编码规则
    /// </summary>
    public class RegistrationNo18Validator
    {
        #region fields
        #region 基础数据
        /// <summary>
        /// 登记管理部门代码标志
        /// </summary>
        private static readonly Dictionary<char, string> ManagementDictionary = new Dictionary<char, string>()
        {
            { '1',"机构编制"},
            { '2',"外交"},
            { '3',"教育"},
            { '4',"公安"},
            { '5',"民政"},
            { '6',"司法"},
            { '7',"交通运输"},
            { '8',"文化"},
            { '9',"工商"},
            { 'A',"旅游局"},
            { 'B',"宗教事务管理"},
            { 'C',"全国总工会"},
            { 'D',"人民解放军总后勤部"},
            { 'E',"省级人民政府"},
            { 'F',"地、市（设区）级人民政府"},
            { 'G',"区、县级人民政府"},
            { 'Y',"其它"},
        };
        /// <summary>
        /// 登记管理部门下组织机构代码，如字典不包含，则默认取1
        /// </summary>
        private static readonly Dictionary<char, Dictionary<char, string>> OrgCodeDictionary = new Dictionary<char, Dictionary<char, string>>
        {
            {'1',new Dictionary<char, string>{ { '1', "机关" }, { '2', "事业单位" }, { '3', "中央编办直接管理机构编制的群众团体" }, { '9', "其它" }, } },
            {'5',new Dictionary<char, string>{ { '1', "社会团体" }, { '2', "民办非企业单位" }, { '3', "基金会" }, { '9', "其它" }, } },
            {'9',new Dictionary<char, string>{ { '1', "企业" }, { '2', "个体工商户" }, { '3', "农民专业合作社" }, } },
        };
        /// <summary>
        /// 本体代码及其对应的代码字符数值
        /// </summary>
        internal static readonly Dictionary<char, int> CodeDictionary = new Dictionary<char, int>();
        /// <summary>
        /// 加权因子
        /// </summary>
        internal static readonly int[] WeightingFactors = { 1, 3, 9, 27, 19, 26, 16, 17, 20, 29, 25, 13, 8, 24, 10, 30, 28 };
        #endregion
        #endregion
        #region oct
        static RegistrationNo18Validator()
        {
            for (var i = 0; i <= 30; i++)
            {
                int num;
                if (i > 27) { num = 59; }
                else if (i > 25) { num = 58; }
                else if (i > 22) { num = 57; }
                else if (i > 17) { num = 56; }
                else if (i > 9) { num = 55; }
                else { num = 48; }
                var c = (char)(num + i);
                CodeDictionary.Add(c, i);
            }
        }
        #endregion
    }
}
