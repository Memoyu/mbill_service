namespace mbill_service.Service.Bill.Statement.Output
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
