using System.ComponentModel;

namespace Memo.Bill.Application.Common.Extensions;
public static class EnumExtenions
{
    public static string GetDescription(this Enum val)
    {
        var field = val.GetType().GetField(val.ToString());
        if (field == null) return string.Empty;
        var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return customAttribute == null ? val.ToString() : ((DescriptionAttribute)customAttribute).Description;
    }
}
