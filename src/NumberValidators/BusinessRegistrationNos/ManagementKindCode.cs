using System;
using System.Collections.Generic;
using System.Text;

namespace NumberValidators.BusinessRegistrationNos
{
    /// <summary>
    /// 登记管理部门下机构类别代码标志,前两位对应ManagementCode，后两位为机构类别ASCII代码，未列举的登记管理部门则其机构类别标志统一使用NonSpecific
    /// </summary>
    public enum ManagementKindCode
    {
        /// <summary>
        /// 未指定类别
        /// </summary>
        NonSpecific = 49,
        /// <summary>
        /// 机构编制  --  机关
        /// </summary>
        Office = 4949,
        /// <summary>
        /// 机构编制  --  事业单位
        /// </summary>
        Institution = 4950,
        /// <summary>
        /// 机构编制  --  中央编办直接管理机构编制的群众团体
        /// </summary>
        MassOrganizations = 4951,
        /// <summary>
        /// 机构编制  --  其它
        /// </summary>
        InstitutionOther = 4957,
        /// <summary>
        /// 民政  --  社会团体
        /// </summary>
        SocialGroups = 5349,
        /// <summary>
        /// 民政  --  民办非企业单位
        /// </summary>
        PeopleRunNonEnterpriseUnits = 5350,
        /// <summary>
        /// 民政  --  基金会
        /// </summary>
        Foundation = 5351,
        /// <summary>
        /// 民政  --  其它
        /// </summary>
        CivilOther = 5357,
        /// <summary>
        /// 工商  --  企业
        /// </summary>
        Enterprise = 5749,
        /// <summary>
        /// 工商  --  个体工商户
        /// </summary>
        IndividualBusinesses = 5750,
        /// <summary>
        /// 工商  --  农民专业合作社
        /// </summary>
        FarmerCooperatives = 5751,
    }
}
