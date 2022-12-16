using Hellang.Middleware.ProblemDetails;
using PSL.MicroserviceTemplate.Api.Common;
using System.Text.Json;

namespace PSL.MicroserviceTemplate.Api.Configuration;

public static class ProblemDetailsConfigurationExtensions
{
    public static IServiceCollection AddProblemDetailsConfiguration(this WebApplicationBuilder builder)
    {
        return builder.Services.AddProblemDetails(options =>
        {
            // Only include exception details in a development and staging environments. Default: Development
            options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment() || builder.Environment.IsStaging();

            // Uses CorrelationId from the request which is assigned to the TraceIdentifier via the CorrelationId Middleware
            options.GetTraceId = (ctx) => ctx.TraceIdentifier;

            // Override the traceId property name
            options.TraceIdPropertyName = JsonNamingPolicy.CamelCase.ConvertName(nameof(CorrelationId));

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });
    }
}
