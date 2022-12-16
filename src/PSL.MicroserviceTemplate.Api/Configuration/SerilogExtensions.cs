using Serilog;

namespace PSL.MicroserviceTemplate.Api.Configuration;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        var openTelemetrySettings = builder.Configuration.GetOpenTelemetryConfiguration();

        builder.Host.UseSerilog((ctx, cfg) =>
        {
            cfg.Enrich.FromLogContext().ReadFrom.Configuration(ctx.Configuration);
            if (openTelemetrySettings.Enabled)
            {
                cfg.Enrich.WithOpenTelemetrySpan();
            }
        });

        return builder;
    }
}
