using System.Net;
using IP2Region.Net.Abstractions;
using Memo.Bill.Application.Common.Interfaces.Services.Region;

namespace Memo.Bill.Infrastructure.Services.Region;

[AppService(ServiceLifeType = ServiceLifeType.Singleton)]
public class RegionSearchService(ISearcher searcher) : IRegionSearchService
{


    public string Search(string ipStr)
    {
        return searcher.Search(ipStr) ?? string.Empty;
    }

    public string Search(IPAddress ipAddress)
    {
        return searcher.Search(ipAddress) ?? string.Empty;
    }

    public RegionInfo SearchInfo(string ipStr)
    {
        var region = searcher.Search(ipStr);
        return ToRegionInfo(region);
    }

    public RegionInfo SearchInfo(IPAddress ipAddress)
    {
        var region = searcher.Search(ipAddress);
        return ToRegionInfo(region);
    }

    public RegionInfo ToRegionInfo(string? region)
    {
        if (string.IsNullOrWhiteSpace(region)) return new();
        var regions = region.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? [];
        if (regions.Count < 5) return new();
        return new RegionInfo
        {
            Country = regions[0] == "0" ? string.Empty : regions[0],
            Region = regions[1] == "0" ? string.Empty : regions[1],
            Province = regions[2] == "0" ? string.Empty : regions[2],
            City = regions[3] == "0" ? string.Empty : regions[3],
            Isp = regions[4] == "0" ? string.Empty : regions[4],
        };
    }
}
