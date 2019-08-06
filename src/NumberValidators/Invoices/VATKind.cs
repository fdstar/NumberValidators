using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberValidators.Invoices
{
    /// <summary>
    /// 增值税发票类别，与Aisino定义一致
    /// </summary>
    public enum VATKind
    {
        /// <summary>
        /// 增值税专用发票
        /// </summary>
        Special = 0,
        /// <summary>
        /// 增值税普通发票(纸质非卷票)
        /// </summary>
        Plain = 2,
        /// <summary>
        /// 货物运输业增值税专用发票（20160701后已停用 http://www.chinatax.gov.cn/n810341/n810755/c1983655/content.html）
        /// </summary>
        Transport = 11,
        /// <summary>
        /// 增值税普通发票（卷票）
        /// </summary>
        Roll = 41,
        /// <summary>
        /// 增值税电子普通发票
        /// </summary>
        Electronic = 51,
        /// <summary>
        /// 区块链电子发票
        /// </summary>
        Blockchain = 71,
    }
    /// <summary>
    /// 增值税电子发票细分类型
    /// </summary>
    public enum ElectronicVATKind
    {
        /// <summary>
        /// 普通的增值税电子发票
        /// </summary>
        Normal = 11,
        /// <summary>
        /// 收费公路通行费
        /// </summary>
        TollRoad = 12,
    }
}
