namespace Memo.Bill.Application.Common.Models.Settings;

public class AuthorizationSettings
{
    public JwtOptions Jwt { get; set; } = new();

    public QiniuOptions Qiniu { get; set; } = new();

    public GitHubOptions GitHub { get; set; } = new();

    public AmapOptions Amap { get; set; } = new();

    public MiniProgramOptions MiniProgram { get; set; } = new();
}
