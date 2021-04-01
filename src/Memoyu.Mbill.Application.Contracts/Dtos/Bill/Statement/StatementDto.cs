/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
*   文件名称 ：StatementDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 23:48:17
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

namespace Memoyu.Mbill.Application.Contracts.Dtos.Bill.Statement
{
    public class StatementDto : MapDto
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }

        public long AssetId { get; set; }

        public long? TargetAssetId { get; set; }

        public decimal Amount { get; set; }

        public decimal AssetResidue { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string Address { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public string Time { get; set; }
    }
}
