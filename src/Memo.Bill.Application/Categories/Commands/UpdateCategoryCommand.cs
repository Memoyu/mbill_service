using Memo.Bill.Application.Common.Security;

namespace Memo.Bill.Application.Categories.Commands;

[Authorize(Permissions = ApiPermission.Category.Update)]
[Transactional]
public record UpdateCategoryCommand(long CategoryId, string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("分类名称不能为空");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("分类图标不能为空");
    }
}

public class UpdateBillCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var entity = await categoryRepo.Select.Where(x => x.CategoryId == request.CategoryId && x.CreateUserId == userId).FirstAsync(cancellationToken)
            ?? throw new ApplicationException("分类不存在或已删除");

        var exist = await categoryRepo.Select.AnyAsync(x => x.Name == request.Name && x.CategoryId != request.CategoryId && x.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("分类已存在");

        var update = mapper.Map<Category>(request);
        update.Id = entity.Id;
        var row = await categoryRepo.UpdateAsync(update, cancellationToken);

        return row > 0 ? Result.Success() : throw new ApplicationException("更新分类失败");
    }
}