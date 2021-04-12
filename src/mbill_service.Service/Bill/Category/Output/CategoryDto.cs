using mbill_service.Core.Domains.Common.Base;

namespace mbill_service.Service.Bill.Category.Output
{
    public class CategoryDto : EntityDto
    {
        public string Name { get; set; }

        public long ParentId { get; set; }

        public string Type { get; set; }

        public decimal Budget { get; set; }

        public string IconUrl { get; set; }
    }
}
