namespace mbill.Service.Bill.Bill.Output;

public class BaseTrendDto
{
    public List<string> Categories { get; set; } = new();

    public List<Serie> Series { get; set; } = new();
}

public class Serie
{
    public string Name { get; set; }

    public List<decimal> Data { get; set; } = new();
}