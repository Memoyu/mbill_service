namespace Mbill.Service.PreOrder.Output;

public class PreOrderGroupWithPreAmountDto : PreOrderGroupDto
{
    public decimal PreAmount { get; set; }

    public decimal RealAmount { get; set; }

    public string PreAmountFormate { get; set; }

    public string RealAmountFormate { get; set; }
}
