using Yitter.IdGenerator;

namespace Memo.Bill.Application.Common.Utils;

public static class SnowFlakeUtil
{
    /// <summary>
    /// 初始化雪花ID生成器
    /// </summary>
    public static void Init()
    {
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
        {
            SeqBitLength = 10
        });
    }

    /// <summary>
    /// 获取雪花ID
    /// </summary>
    /// <returns></returns>
    public static long NextId()
    {
        return YitIdHelper.NextId();
    }
}
