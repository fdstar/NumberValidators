using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos
{
    /// <summary>
    /// 登记管理部门代码标志，对应ASCII
    /// </summary>
    public enum ManagementCode
    {
        /// <summary>
        /// 机构编制 (1)
        /// </summary>
        Institution = 49,
        /// <summary>
        /// 外交 (2)
        /// </summary>
        Diplomacy = 50,
        /// <summary>
        /// 教育 (3)
        /// </summary>
        Education = 51,
        /// <summary>
        /// 公安 (4)
        /// </summary>
        Police = 52,
        /// <summary>
        /// 民政 (5)
        /// </summary>
        Civil = 53,
        /// <summary>
        /// 司法 (6)
        /// </summary>
        Justice = 54,
        /// <summary>
        /// 交通运输 (7)
        /// </summary>
        Transportation = 55,
        /// <summary>
        /// 文化 (8)
        /// </summary>
        Culture = 56,
        /// <summary>
        /// 工商 (9)
        /// </summary>
        Business = 57,
        /// <summary>
        /// 旅游局 (A)
        /// </summary>
        Tourism = 65,
        /// <summary>
        /// 宗教事务管理 (B)
        /// </summary>
        Religion = 66,
        /// <summary>
        /// 全国总工会 (C)
        /// </summary>
        ACFTU = 67,
        /// <summary>
        /// 人民解放军总后勤部 (D)
        /// </summary>
        ArmyLogisticsDepartment = 68,
        /// <summary>
        /// 省级人民政府 (E)
        /// </summary>
        ProvincialGovernment = 69,
        /// <summary>
        /// 地、市（设区）级人民政府 (F)
        /// </summary>
        MunicipalGovernment = 70,
        /// <summary>
        /// 区、县级人民政府 (G)
        /// </summary>
        CountyGovernment = 71,
        /// <summary>
        /// 其它 (Y)
        /// </summary>
        Other = 89,
    }
}
