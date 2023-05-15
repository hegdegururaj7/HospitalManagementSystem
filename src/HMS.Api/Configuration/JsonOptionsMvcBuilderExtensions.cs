using System.Text.Json.Serialization;
using System.Text.Json;

namespace HMS.Service.Api.Configuration;

public static class JsonOptionsMvcBuilderExtensions
{
    public static IMvcBuilder AddJsonOptionsConfiguration(this IMvcBuilder builder)
        => builder.AddJsonOptions(options => options.JsonSerializerOptions.AddJsonOptionsConfiguration());

    public static void AddJsonOptionsConfiguration(this JsonSerializerOptions options)
    {
        // Exclude properties from Json output when their values are null
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }
}