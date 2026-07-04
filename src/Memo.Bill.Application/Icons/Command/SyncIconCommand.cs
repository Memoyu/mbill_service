using Memo.Bill.Domain.Events.FileStorages;

namespace Memo.Bill.Application.Icons.Command;

[Authorize(Permissions = ApiPermission.Icon.Sync)]
public record SyncIconCommand : IAuthorizeableRequest<Result>;

public class SyncIconCommandCommandHandler(IPublisher publisher) : IRequestHandler<SyncIconCommand, Result>
{
    public async Task<Result> Handle(SyncIconCommand request, CancellationToken cancellationToken)
    {
        await publisher.Publish(new SyncQiniuIconResourceEvent());
        return Result.Success();
    }
}