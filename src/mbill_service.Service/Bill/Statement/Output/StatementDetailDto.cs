namespace mbill_service.Service.Bill.Statement.Output
{
    public class StatementDetailDto : StatementDto
    {
        public string categoryParentName { get; set; }

        public string assetParentName { get; set; }
    }
}
