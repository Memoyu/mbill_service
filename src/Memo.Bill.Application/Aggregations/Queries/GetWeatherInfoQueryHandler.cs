using EasyCaching.Core;
using Memo.Bill.Application.Aggregations.Common;
using Memo.Bill.Application.Common.Interfaces.Services.Amap;

namespace Memo.Bill.Application.Aggregations.Queries;

[Authorize(Permissions = ApiPermission.Aggregation.GetWeatherInfo)]
public record GetWeatherInfoQuery(string City) : IAuthorizeableRequest<Result>;

public class GetWeatherInfoQueryHandler(
    IMapper mapper,
    IEasyCachingProvider ecProvider,
    IAmapService amapService) : IRequestHandler<GetWeatherInfoQuery, Result>
{
    public async Task<Result> Handle(GetWeatherInfoQuery request, CancellationToken cancellationToken)
    {
        var city = request.City.Trim();
        var key = CacheKeyConst.WeatherInfo(city);
        if (await ecProvider.ExistsAsync(key, cancellationToken))
            return Result.Success(ecProvider.GetAsync<WeatherInfoResult>(key, cancellationToken));
        
        var res = await amapService.GetWeatherInfoAsync(city, cancellationToken);
        var dto = mapper.Map<WeatherInfoResult>(res);
        // 将天气信息缓存6小时
        await ecProvider.SetAsync(key, dto, TimeSpan.FromHours(6), cancellationToken);
        return Result.Success(dto);
    }
}
