using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PSL.MicroserviceTemplate.IntegrationTests.Helpers;

public static class ResponseExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions;

    static ResponseExtensions()
    {
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
        };
    }

    public static string AsString(this HttpContent content)
    {
        try
        {
            var readTask = content.ReadAsStringAsync();
            readTask.Wait();

            if (readTask.IsCompletedSuccessfully)
                return readTask.Result;

            throw new Exception("Could not extract content as string from HttpContent");
        }
        catch
        {
            throw;
        }
    }

    public static T Deserialize<T>(this HttpContent content)
    {
        try
        {
            var json = content.AsString();
            return JsonSerializer.Deserialize<T>(json, _serializerOptions);
        }
        catch
        {
            throw;
        }
    }
}
