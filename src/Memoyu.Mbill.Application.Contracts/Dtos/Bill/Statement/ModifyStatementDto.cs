/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
*   文件名称 ：ModifyStatementDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 23:48:08
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.ComponentModel.DataAnnotations;

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class ModifyStatementDto
    {
        /// <summary>
        /// 资产Id
        /// </summary>
        [Required(ErrorMessage = "必须传入源资产Id")]
        public long AssetId { get; set; }

        /// <summary>
        /// 目标资产Id
        /// </summary>
        public long? TargetAssetId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Required(ErrorMessage = "金额不能为空")]
        [Range(0.01, 100000, ErrorMessage = "金额应该在0.01-100000之间")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>

        public decimal AssetResidue { get; set; }

        /// <summary>
        /// 记录类型：支出、收入、转账、还款
        /// </summary>
        [Required(ErrorMessage = "必须传入账单类型")]
        public string Type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 记录日期：年
        /// </summary>
        [Required(ErrorMessage = "必须传入记录日期：年")]
        [Range(1997, 3030, ErrorMessage = "日期年应该在1997-3030之间")]
        public int Year { get; set; }

        /// <summary>
        /// 记录日期：月
        /// </summary>
        [Required(ErrorMessage = "必须传入记录日期：月")]
        [Range(1, 12, ErrorMessage = "日期月应该在1-12之间")]
        public int Month { get; set; }

        /// <summary>
        /// 记录日期：日
        /// </summary>
        [Required(ErrorMessage = "必须传入记录日期：日")]
        [Range(1, 31, ErrorMessage = "日期日应该在1-31之间")]
        public int Day { get; set; }

        /// <summary>
        /// 记录日期：时间
        /// </summary>
        [Required(ErrorMessage = "必须传入记录日期：时间")]
        public string Time { get; set; }
    }
}
