using Memo.Bill.Domain.Entities;

namespace Memo.Bill.Domain.Events.Aggregations;

public record UserGetLocationEvent(UserLocation UserLocation) : IDomainEvent;
