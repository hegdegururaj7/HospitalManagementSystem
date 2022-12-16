using PSL.MicroserviceTemplate.Api.Common;

namespace PSL.MicroserviceTemplate.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(CorrelationId.RequestHeaderKey, out var correlationId))
        {
            context.TraceIdentifier = CorrelationId.Parse(correlationId);
        }

        // Apply the correlation ID to the response header for client side tracking
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(CorrelationId.RequestHeaderKey, new[]
            {
            context.TraceIdentifier
        });

            return Task.CompletedTask;
        });

        return _next(context);
    }
}
