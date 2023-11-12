using Mbill.Core.Common;

namespace Mbill.Service.Core.DataSeed.Output;

public class BillCategoryDataSeedDto
{
    public string Name { get; set; }

    public int Type { get; set; }

    public int Sort { get; set; }

    public string Icon { get; set; } = string.Empty;

    public List<BillCategoryDataSeedDto> Childs { get; set; }

    public CategoryEntity ToEntity(long? bId, long? parent, long userBId)
    {
        return new CategoryEntity
        {
            BId = bId.HasValue ? bId.Value : SnowFlake.NextId(),
            ParentBId = parent.HasValue ? parent.Value : 0,
            Name = Name,
            Type = Type,
            Sort = Sort,
            Icon = parent.HasValue ? Icon : string.Empty,
            CreateUserBId = userBId,
        };
    }
}
