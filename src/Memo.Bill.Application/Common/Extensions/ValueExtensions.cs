namespace Memo.Bill.Application.Common.Extensions;

public static class ValueExtensions
{
    /// <summary>
    /// 保留指定小数位，默认保留两位小数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <returns></returns>
    public static decimal ToRound(this decimal value, int decimals = 2) => Math.Round(value, decimals);
}
