using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Memo.Bill.Application.Common.Extensions;

public static class FormatExtensions
{
    private static readonly JsonSerializerOptions _defaultOption = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    static FormatExtensions()
    {
        _defaultOption.Converters.Add(new AutoLongToStringConverter());
    }

    public static string ToJson(this object obj, JsonSerializerOptions? options = null)
    {
        if (obj == null) return string.Empty;
        options ??= _defaultOption;
        return JsonSerializer.Serialize(obj, options);
    }

    public static T? ToDesJson<T>(this string json, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(json)) return default;
        options ??= _defaultOption;
        return JsonSerializer.Deserialize<T>(json, options);
    }
}

public class AutoLongToStringConverter : JsonConverter<object>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(long) == typeToConvert || typeof(long?) == typeToConvert;
    }

    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 目标类型为long 或 long?
        if (typeof(long) == typeToConvert || typeof(long?) == typeToConvert)
        {
            // 输入为数值
            if (reader.TokenType == JsonTokenType.Number)
            {
                reader.TryGetInt64(out long l);
                return l;
            }

            // 输入为字符串
            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                return long.TryParse(s, out long value) ? value : typeof(long?) == typeToConvert ? null : 0;
            }
        }

        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            throw new Exception($"unable to parse {document.RootElement.ToString()} to number");
        }
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
