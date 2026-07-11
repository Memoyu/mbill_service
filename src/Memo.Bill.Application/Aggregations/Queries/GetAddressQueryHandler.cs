using EasyCaching.Core;
using Memo.Bill.Application.Aggregations.Common;
using Memo.Bill.Application.Common.Interfaces.Services.Amap;
using Memo.Bill.Domain.Events.Aggregations;

namespace Memo.Bill.Application.Aggregations.Queries;

[Authorize(Permissions = ApiPermission.Aggregation.GetLocation)]
public record GetAddressQuery(string Location) : IAuthorizeableRequest<Result>;

public class GetAddressQueryValidator : AbstractValidator<GetAddressQuery>
{
    public GetAddressQueryValidator()
    {
        RuleFor(x => x.Location)
           .NotEmpty()
           .WithMessage("经纬度不能为空");
    }
}

public class GetAddressQueryHandler(
    IMapper mapper,
    IPublisher publisher,
    IAmapService amapService,
    IEasyCachingProvider ecProvider,
    ICurrentUserProvider currentUserProvider) : IRequestHandler<GetAddressQuery, Result>
{
    public async Task<Result> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var location = request.Location.Trim();

        var key = CacheKeyConst.AddressInfo(location);
        var cacheValue = await ecProvider.GetAsync<AddressResult>(key, cancellationToken);
        if (cacheValue.HasValue)
            return Result.Success(cacheValue.Value);

        var userId = currentUserProvider.GetCurrentUser().Id;

        var res = await amapService.GetGeocodeRegeoAsync(location, cancellationToken);
        var address = mapper.Map<AddressResult>(res);
        await ecProvider.SetAsync(key, address, TimeSpan.FromMinutes(3), cancellationToken);
        var userLocation = mapper.Map<UserLocation>(address);

        userLocation.UserId = userId;
        userLocation.Location = location;
        await publisher.Publish(new UserGetLocationEvent(userLocation), cancellationToken);

        return Result.Success(address);
    }
}
