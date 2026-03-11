namespace Memo.Bill.Application.Categories.Commands;

[Authorize(Permissions = ApiPermission.Category.Create)]
[Transactional]
public record CreateCategoryCommand(string Name, string Icon, bool IsDefault, long? ParentId) : IAuthorizeableRequest<Result>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("分类名称不能为空");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("分类图标不能为空");
    }
}

public class CreateCategoryCommandHandler(
    IMapper mapper,
    IBaseDefaultRepository<Category> categoryRepo
    ) : IRequestHandler<CreateCategoryCommand, Result>
{
    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exist = await categoryRepo.Select.AnyAsync(x => x.Name == request.Name, cancellationToken);
        if (exist) return Result.Failure("分类已存在");

        var entity = mapper.Map<Category>(request);
        entity = await categoryRepo.InsertAsync(entity, cancellationToken);
        if (entity.Id <= 0) throw new ApplicationException("保存分类失败");

        return Result.Success(entity.CategoryId);
    }
}