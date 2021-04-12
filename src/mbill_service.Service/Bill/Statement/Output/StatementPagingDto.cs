using mbill_service.Core.Domains.Common;

namespace mbill_service.Service.Bill.Statement.Output
{
    public class StatementPagingDto : PagingDto
    {
        public long? UserId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Day { get; set; }

        public string  Type { get; set; }

        public long? CategoryId { get; set; }

        public long? AssetId { get; set; }

    }
}
