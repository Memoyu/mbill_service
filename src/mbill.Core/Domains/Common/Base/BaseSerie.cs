namespace Mbill.Core.Domains.Common.Base;

public class BaseSerie : BaseSerie<decimal>
{
}

public class BaseSerie<T>
{
    public string Name { get; set; }

    public List<T> Data { get; set; } = new();
}
