namespace mbill.Service.Bill.Bill.Output;

public class CategoryPercentStatDto: BaseChart<RingSerie>
{

}

public class RingSerie
{
    public string Name { get; set; }

    public decimal Value { get; set; }
}
