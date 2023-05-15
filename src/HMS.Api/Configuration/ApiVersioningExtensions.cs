using Microsoft.AspNetCore.Mvc.Versioning;

namespace HMS.Service.Api.Configuration;

public static class ApiVersioningServiceCollectionExtensions
{
    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(cfg =>
        {
            cfg.DefaultApiVersion = new ApiVersion(1, 0);
            cfg.AssumeDefaultVersionWhenUnspecified = true;
            cfg.ReportApiVersions = true;
            cfg.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                            new HeaderApiVersionReader("x-api-version"),
                                                            new MediaTypeApiVersionReader("x-api-version"));
        });

        services.AddVersionedApiExplorer(cfg =>
        {
            cfg.GroupNameFormat = "'v'VVV";
            cfg.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
