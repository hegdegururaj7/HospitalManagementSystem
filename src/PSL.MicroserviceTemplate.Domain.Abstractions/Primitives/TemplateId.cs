using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PSL.MicroserviceTemplate.Domain.Primitives;

[JsonConverter(typeof(TemplateIdJsonConverter))]
[TypeConverter(typeof(TemplateIdTypeConverter))]
public readonly record struct TemplateId : IPrimitive<Guid>, IEquatable<TemplateId>
{
    public TemplateId(Guid value)
    {
        Value = value;
    }

    private TemplateId(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            Value = guid;
        }
    }

    public Guid Value { get; } = Guid.Empty;

    public bool HasValue => Value != Guid.Empty;

    public const string Pattern = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";

    public bool Equals(TemplateId other) => Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString("D");

    public static implicit operator string(TemplateId value) => value.ToString();
    public static implicit operator Guid(TemplateId value) => value.Value;
    public static implicit operator TemplateId(Guid value) => new(value);
    public static implicit operator TemplateId(string value) => new(value);

    public static TemplateId New() => Guid.NewGuid();

    public static TemplateId Parse(string value) => new(value);

    /// <summary>
    /// Convert a serialised Guid string into a RxItemId
    /// </summary>
    /// <param name="value">A serialised Guid string</param>
    /// <param name="TemplateId">When this method returns, contains the TemplateId 
    /// created from the parsed value. If the method returns true, result contains 
    /// a valid TemplateId. If the method returns false, the result has a default 
    /// value derived from an Empty Guid.</param>
    /// <returns>true parsing the value was successful, or 
    /// false the value could not be parsed.</returns>
    public static bool TryParse(string value, out TemplateId TemplateId)
    {
        if (Guid.TryParse(value, out var guid))
        {
            TemplateId = new TemplateId(guid);
            return true;
        }

        TemplateId = Empty;
        return false;
    }

    public static readonly TemplateId Empty = new(Guid.Empty);
}

public class TemplateIdJsonConverter : JsonConverter<TemplateId>
{
    public override TemplateId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => TemplateId.Parse(reader.GetString());

    public override void Write(Utf8JsonWriter writer, TemplateId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class TemplateIdTypeConverter : TypeConverter
{
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) => value switch
    {
        string @string => TemplateId.Parse(@string),
        Guid guid => new TemplateId(guid),
        _ => new TemplateId()
    };

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) ||
        sourceType == typeof(Guid?) ||
        sourceType == typeof(Guid);
}