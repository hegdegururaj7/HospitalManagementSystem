using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PSL.MicroserviceTemplate.Api.Configuration;

public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Try and add OpenTelemetry if Enabled
    /// </summary>
    /// <param name="webApplicationBuilder">Instance of <see cref="WebApplicationBuilder"/></param>
    /// <returns>Instance of <see cref="WebApplicationBuilder"/> with OpenTelemetry configured if enabled</returns>
    public static WebApplicationBuilder TryAddOpenTelemetry(this WebApplicationBuilder webApplicationBuilder)
    {
        var serviceName = webApplicationBuilder.Configuration["Serilog:Properties:ApplicationName"];
        var openTelemetrySettings = webApplicationBuilder.Configuration.GetOpenTelemetryConfiguration();
        if (openTelemetrySettings.Enabled)
        {
            webApplicationBuilder.Services.AddOpenTelemetry(openTelemetrySettings, serviceName);
        }

        return webApplicationBuilder;
    }


    /// <summary>
    /// Let's configure OpenTelemetry
    /// </summary>
    /// <param name="services"></param>
    /// <param name="openTelemetrySettings"></param>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services,
                                                      OpenTelemetrySettings openTelemetrySettings,
                                                      string serviceName)
    {
        services.AddOpenTelemetry()
                .WithTracing(tpb =>
                {
                    tpb.AddSource(serviceName)
                       .SetResourceBuilder(ResourceBuilder.CreateDefault()
                                                          .AddService(serviceName: serviceName)
                                                          .AddTelemetrySdk()
                                                          .AddEnvironmentVariableDetector())
                       .AddSource("MassTransit")
                       .AddAspNetCoreInstrumentation(options =>
                       {
                           options.Filter = httpContext =>
                           {
                               // excludes healthcheck from open telemetry collection
                               var healthPath = new PathString("/health");
                               var method = httpContext.Request.Method;
                               var path = httpContext.Request.Path;
                               return !(method == "GET" && path.StartsWithSegments(healthPath));
                           };
                           options.RecordException = true;
                       })
                       .AddHttpClientInstrumentation((options) =>
                       {
                           options.RecordException = true;
                       })
                       .AddOtlpExporter(options => options.Endpoint = new Uri(openTelemetrySettings.Address));
                })
                .StartWithHost();

        services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));

        return services;
    }

    /// <summary>
    /// Retrieves OpenTelemetry settings
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="InvalidConfigurationException"></exception>
    public static OpenTelemetrySettings GetOpenTelemetryConfiguration(this IConfiguration configuration)
    {
        var settings = configuration.GetSection("OpenTelemetry").Get<OpenTelemetrySettings>();

        if (settings == null)
        {
            throw new InvalidConfigurationException("OpenTelemetry section doesn't exist");
        }

        if (settings.Enabled && string.IsNullOrEmpty(settings.Address))
        {
            throw new InvalidConfigurationException("OpenTelemetry:Address must not be null or empty");
        }

        return settings;
    }
}