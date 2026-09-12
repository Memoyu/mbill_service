namespace Memo.Bill.Application.Common.Models;

public record NameValueDto<T>
{
    public string Name { get; set; } = string.Empty;

    public required T Value { get; set; }
}
