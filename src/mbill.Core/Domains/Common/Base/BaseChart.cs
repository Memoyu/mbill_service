namespace mbill.Core.Domains.Common.Base;

public class BaseChart<T>
{
    public List<string> Categories { get; set; } = new();

    public List<T> Series { get; set; } = new();
}
