namespace Memo.Bill.Application.FileStorages.Queries.Generate;

[Authorize(Permissions = ApiPermission.FileStorage.GenerateQiniuUploadToken)]
public record GenerateQiniuUploadTokenQuery(string Path) : IAuthorizeableRequest<Result>;

public class GenerateQiniuUploadTokenQueryValidator : AbstractValidator<GenerateQiniuUploadTokenQuery>
{
    public GenerateQiniuUploadTokenQueryValidator()
    {
        RuleFor(x => x.Path)
            .NotEmpty()
            .WithMessage("上传路径不能为空");
    }
}
