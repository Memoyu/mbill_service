using Mapster;

namespace Mbill.Service.Common.Registers;

public abstract class BaseRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeRegister(config);
    }

    protected abstract void TypeRegister(TypeAdapterConfig config);

    public string UrlConverter(string url)
    {
        if (url.IsNullOrWhiteSpace()) return "";
        var fileRepo = MapContext.Current.GetService<IFileRepo>();
        return fileRepo.GetFileUrl(url);
    }

    public string GenderConverter(int gender) => gender == 0 ? "未知" : gender == 1 ? "男" : "女";

    public string CategoryTypeConverter(int type) => Switcher.CategoryType(type);
}
