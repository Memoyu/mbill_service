using Memo.Bill.Application.Common.Models.Settings;
using Memo.Bill.Application.Common.Utils.QiniuUtil;
using Memo.Bill.Application.Common.Utils.QiniuUtil.QiniuUtil;
using Memo.Bill.Application.FileStorages.Common;
using Microsoft.Extensions.Options;

namespace Memo.Bill.Application.FileStorages.Command;

[Authorize(Permissions = ApiPermission.FileStorage.CreateQiniuUploadToken)]
public record CreateQiniuUploadTokenCommand(string Path) : IAuthorizeableRequest<Result>;

public class CreateQiniuUploadTokenCommandValidator : AbstractValidator<CreateQiniuUploadTokenCommand>
{
    public CreateQiniuUploadTokenCommandValidator()
    {
        RuleFor(x => x.Path)
            .NotEmpty()
            .WithMessage("上传路径不能为空");
    }
}

public class CreateQiniuUploadTokenCommandHandler(IOptionsMonitor<AuthorizationSettings> authOptions) : IRequestHandler<CreateQiniuUploadTokenCommand, Result>
{
    public async Task<Result> Handle(CreateQiniuUploadTokenCommand request, CancellationToken cancellationToken)
    {
        var options = authOptions.CurrentValue?.Qiniu ?? throw new Exception("未配置七牛云授权信息");
        var sign = new QiniuSignature(options.AK, options.SK);
        var policy = new QiniuPutPolicy
        {
            Scope = string.IsNullOrWhiteSpace(request.Path) ? options.Bucket : $"{options.Bucket}:{request.Path}"
        };

        var token = sign.SignWithData(policy.ToJsonString());

        return await Task.FromResult(Result.Success(new QiniuUploadTokenResult { Token = token, Host = options.Host }));
    }
}

