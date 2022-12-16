using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PSL.MicroserviceTemplate.Domain.Primitives;

namespace PSL.MicroserviceTemplate.Api.Configuration;

public static class SwaggerConfigurationExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(cfg =>
        {
            cfg.MapType<TemplateId>(() => new OpenApiSchema { Type = "string", Format = "uuid", Pattern = TemplateId.Pattern, Example = new OpenApiString("7241e353-6c40-4774-9ff0-0af7484e9f9a") });

        });

        return services;
    }
}
