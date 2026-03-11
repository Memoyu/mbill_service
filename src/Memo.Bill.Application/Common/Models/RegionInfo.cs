namespace Memo.Bill.Application.Common.Models;

public class RegionInfo
{
    public string Country { get; set; }  = string.Empty;

    public string Region { get; set; } = string.Empty;

    public string Province { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Isp { get; set; } = string.Empty;

    public string GetRegion() =>string.IsNullOrWhiteSpace(Country) ? string.Empty : $"{Country}-{Region}{Province}{City}";
}
