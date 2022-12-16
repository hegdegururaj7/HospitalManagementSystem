using PSL.MicroserviceTemplate.Api.Middleware;

namespace PSL.MicroserviceTemplate.Api.Configuration;

public static class CorrelationIdApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
