using Memo.Bill.Application.Common.Models.Settings;
using Memo.Bill.Application.Common.Utils.QiniuUtil;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace Memo.Bill.Application.FileStorages.Queries;

public record ListFileQuery() : IAuthorizeableRequest<Result>;

public class ListGroupIconQueryHandler(
    IMapper mapper,
    IOptionsMonitor<AuthorizationSettings> authOptions,
    IHttpClientFactory httpClientFactory
    ) : IRequestHandler<ListFileQuery, Result>
{
    public async Task<Result> Handle(ListFileQuery request, CancellationToken cancellationToken)
    {
        var options = authOptions.CurrentValue?.Qiniu ?? throw new Exception("未配置七牛云授权信息");
        var sign = new QiniuSignature(options.AK, options.SK);

        var path = $"/list?bucket={options.Bucket}&delimiter=icons";
        var date = DateTime.Now.ToString("yyyyMMddTHHmmssZ"); 
        var signStr = $"GET {path}\nHost: rsf-z2.qbox.me\nContent-Type: application/json\nX-Qiniu-Date: {date}\n\n";

        var token = sign.Sign(signStr);

        var client = httpClientFactory.CreateClient();

        Activity.Current = null;
        using var req = new HttpRequestMessage(HttpMethod.Get, "http://rsf-z2.qbox.me" + path);
        req.Headers.Add("Authorization", $"Qiniu {token}");
        req.Headers.Add("X-Qiniu-Date", date);
        var resp = await client.SendAsync(req);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadAsStringAsync();


        return Result.Success();
    }
}

