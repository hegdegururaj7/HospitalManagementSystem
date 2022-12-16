using PSL.MicroserviceTemplate.Domain.Primitives;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PSL.MicroserviceTemplate.Api.Common;

[JsonConverter(typeof(CorrelationIdJsonConverter))]
[TypeConverter(typeof(CorrelationIdTypeConverter))]
public readonly record struct CorrelationId : IPrimitive<Guid>, IEquatable<CorrelationId>
{
    public const string RequestHeaderKey = "X-Correlation-ID";

    public CorrelationId(Guid value)
    {
        Value = value;
    }

    private CorrelationId(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            Value = guid;
        }
    }

    public bool HasValue => Value != default;

    public Guid Value { get; } = default;

    public bool Equals(CorrelationId other) => Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString("D");

    public static implicit operator string(CorrelationId value) => value.ToString();
    public static implicit operator Guid(CorrelationId value) => value.Value;
    public static implicit operator CorrelationId(Guid value) => new(value);
    public static implicit operator CorrelationId(string value) => new(value);

    public static CorrelationId New() => Guid.NewGuid();

    public static CorrelationId Parse(string value) => new(value);
}

public class CorrelationIdJsonConverter : JsonConverter<CorrelationId>
{
    public override CorrelationId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => CorrelationId.Parse(reader.GetString());

    public override void Write(Utf8JsonWriter writer, CorrelationId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class CorrelationIdTypeConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) => value switch
    {
        string @string => CorrelationId.Parse(@string),
        Guid guid => new CorrelationId(guid),
        _ => new CorrelationId()
    };

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) ||
        sourceType == typeof(Guid?) ||
        sourceType == typeof(Guid);
}
