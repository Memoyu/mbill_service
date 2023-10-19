using Yitter.IdGenerator;

namespace mbill.Core.Common
{
    public static class SnowFlake
    {
        /// <summary>
        /// 初始化雪花ID生成器
        /// </summary>
        public static void SnowFlakeConfig()
        {
            var options = new IdGeneratorOptions();
            options.SeqBitLength = 10;
            YitIdHelper.SetIdGenerator(options);
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
}
