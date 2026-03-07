namespace Memo.Bill.Application.Common.Models.Services.Amap;

public class AmapBaseResponse
{
    /// <summary>
    /// 返回结果状态值 返回值为 0 或 1，0 表示请求失败；1 表示请求成功。
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 返回状态说明，当 status 为 0 时，info 会返回具体错误原因，否则返回“OK”。
    /// </summary>
    public string Info { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Infocode { get; set; } = string.Empty;

    /// <summary>
    /// 请求是否成功
    /// </summary>
    public bool IsSuccess => Status == "1";
}
