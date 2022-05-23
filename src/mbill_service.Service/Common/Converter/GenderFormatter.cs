namespace mbill_service.Service.Common.Common.Converter;

public class GenderFormatter : IValueConverter<int, string>
{
    private readonly IBaseTypeRepo _baseTypeRepo;
    private readonly IBaseItemRepo _baseItemRepo;
    public GenderFormatter(IBaseItemRepo baseItemRepo, IBaseTypeRepo baseTypeRepo)
    {
        _baseItemRepo = baseItemRepo;
        _baseTypeRepo = baseTypeRepo;
    }
    public string Convert(int sourceMember, ResolutionContext context)
    {
        var typeId = _baseTypeRepo.Select.Where(t => t.TypeCode == "Sex").ToOne()?.Id;
        var item = _baseItemRepo.Select.Where(i => i.BaseTypeId == typeId && i.ItemCode == $"{sourceMember}").ToOne();
        return item?.ItemName;
    }
}
