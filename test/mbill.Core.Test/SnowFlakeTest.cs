using mbill.Core.Common;

namespace mbill.Core.Test
{
    public class SnowFlakeTest
    {
        public SnowFlakeTest()
        {
            // ÅäÖÃÑ©»¨IDÉú³ÉÆ÷
            SnowFlake.SnowFlakeConfig();
        }

        [Fact]
        public void Generate_New_Id_Should_Succeed()
        {
            var id = SnowFlake.NextId();
            Assert.Equal(16, id.ToString().Length);
        }

        [Fact]
        public void Parallel_Generate_Should_Succeed()
        {
            var ids = new List<long>(1000);

            Parallel.For(0, 1000, index =>
            {
                ids.Add(SnowFlake.NextId());
            });

            var group = ids.GroupBy(d => d);
            Assert.Equal(0, group.Count(g => g.Count() > 1));
        }
    }
}