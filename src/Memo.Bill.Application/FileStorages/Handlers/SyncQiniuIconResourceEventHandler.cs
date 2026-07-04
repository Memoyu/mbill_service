using Memo.Bill.Application.Common.Models.Settings;
using Memo.Bill.Application.Common.Utils.QiniuUtil;
using Memo.Bill.Application.FileStorages.Common;
using Memo.Bill.Domain.Events.FileStorages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace Memo.Bill.Application.FileStorages.Handlers;

public class SyncQiniuIconResourceEventHandler(
    ILogger<SyncQiniuIconResourceEventHandler> logger,
    IOptionsMonitor<AuthorizationSettings> authOptions,
    IHttpClientFactory httpClientFactory,
    IBaseDefaultRepository<Icon> iconRepo,
    IBaseDefaultRepository<IconCatalog> iconCatalogRepo
    ) : INotificationHandler<SyncQiniuIconResourceEvent>
{
    public async Task Handle(SyncQiniuIconResourceEvent notification, CancellationToken cancellationToken)
    {
        var options = authOptions.CurrentValue?.Qiniu ?? throw new Exception("未配置七牛云授权信息");
        var sign = new QiniuSignature(options.AK, options.SK);

        var path = $"/list?bucket={options.Bucket}&prefix=icons";
        var date = DateTime.UtcNow.ToString("yyyyMMdd'T'HHmmss'Z'");
        var signStr = $"POST {path}\nHost: rsf-z2.qbox.me\nContent-Type: application/x-www-form-urlencoded; charset=utf-8\nX-Qiniu-Date: {date}\n\n";
        var token = sign.Sign(signStr);

        var client = httpClientFactory.CreateClient();

        Activity.Current = null;
        using var req = new HttpRequestMessage(HttpMethod.Post, "http://rsf-z2.qbox.me" + path);
        req.Content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
        req.Headers.Add("Authorization", $"Qiniu {token}");
        req.Headers.Add("X-Qiniu-Date", date);

        var fileItems = new List<QiniuListFileItem>();
        try
        {
            var resp = await client.SendAsync(req, cancellationToken);
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync(cancellationToken);
            var res = json.ToDesJson<QiniuListFileResp>();
            if (res == null || !string.IsNullOrWhiteSpace(res.Error))
                throw new Exception($"七牛云响应错误：{res?.Error ?? json}");
            fileItems = res.Items;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "同步图标资源数据异常");
        }
        if (fileItems.Count < 1) return;

        var newIcons = new List<Icon>();
        var catalogHash = new HashSet<string>();
        var icons = await iconRepo.Select.ToListAsync(cancellationToken);
        foreach (var item in fileItems)
        {
            if (item.Fsize < 1) continue;
            if (icons.Any(i => i.Path == item.Key)) continue;

            var iconPath = item.Key.Replace("icons/", string.Empty);
            var index = iconPath.IndexOf('/');
            var catalog = iconPath.Substring(0, index) ?? iconPath;
            newIcons.Add(new Icon { Catalog = catalog, Host = options.Host, Path = item.Key });
            catalogHash.Add(catalog);
        }

        // 插入图标资源
        if (newIcons.Count > 0)
            await iconRepo.InsertAsync(newIcons, cancellationToken);

        // 同步图标目录
        if (catalogHash.Count > 0)
        {
            var icnCatalogs = await iconCatalogRepo.Select.ToListAsync(cancellationToken);
            var newCatalogs = catalogHash.Where(c => !icnCatalogs.Any(ic => ic.Code == c)).Select(c => new IconCatalog { Code = c }).ToList();
            if (newCatalogs.Count > 0)
                await iconCatalogRepo.InsertAsync(newCatalogs, cancellationToken);
        }
    }
}
