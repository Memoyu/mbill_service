namespace Memo.Bill.Application.Categories.Queries.Get;

public record GetCategoryQuery(long CategoryId) : IAuthorizeableRequest<Result>;

public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("分类Id不能为空");
    }
}
