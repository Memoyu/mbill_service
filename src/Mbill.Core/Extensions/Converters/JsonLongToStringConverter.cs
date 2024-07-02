namespace Mbill.Core.Extensions.Converters;

public class JsonLongToStringConverter : JsonConverter
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(long) == typeToConvert || typeof(long?) == typeToConvert;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if ((reader.ValueType == null || reader.ValueType == typeof(long?)) && reader.Value == null)
        {
            return null;
        }
        else
        {
            long.TryParse(reader.Value != null ? reader.Value.ToString() : "", out long value);
            return value;
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString());
    }
}
