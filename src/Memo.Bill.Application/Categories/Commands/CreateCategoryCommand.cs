using Memo.Bill.Application.Categories.Common;

namespace Memo.Bill.Application.Categories.Commands;

[Authorize(Permissions = ApiPermission.Category.Create)]
public record CreateCategoryCommand(BillType Type, string Name, string? Icon, long? ParentId) : IAuthorizeableRequest<Result>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("分类类型错误");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("分类名称不能为空");
    }
}

public class CreateCategoryCommandHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<CreateCategoryCommand, Result>
{
    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var exist = await categoryRepo.Select.AnyAsync(x => x.Name == request.Name && x.CreateUserId == userId, cancellationToken);
        if (exist) return Result.Failure("分类已存在");

        var maxSort = await categoryRepo.Select
            .Where(c => c.CreateUserId == userId)
            .Where(c => c.ParentId == request.ParentId)
            .MaxAsync(c => c.Sort + 1, cancellationToken);
        var entity = mapper.Map<Category>(request);
        entity.Icon = entity.Icon ?? string.Empty;
        entity.Default = false;
        entity.Sort = maxSort;
        entity = await categoryRepo.InsertAsync(entity, cancellationToken);
        if (entity.Id <= 0) throw new ApplicationException("保存分类失败");

        return Result.Success(mapper.Map<CategoryResult>(entity));
    }
}