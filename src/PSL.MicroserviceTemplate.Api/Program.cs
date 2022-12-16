using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PSL.MicroserviceTemplate.Api.Configuration;
using PSL.MicroserviceTemplate.Api.ModelBinders;
using PSL.MicroserviceTemplate.Data;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
#if DEBUG
    .WriteTo.Debug()
#endif
    .CreateBootstrapLogger();
Log.Information("Creating PSL.MicroserviceTemplate.Api...");

try
{
    Log.Information("Configuring PSL.MicroserviceTemplate.Api...");
    var builder = WebApplication.CreateBuilder(args);

    // Configure Endpoints & Routing
    builder.AddProblemDetailsConfiguration();
    builder.Services.AddControllers(cfg =>
    {
        cfg.Filters.Add(new ProducesAttribute("application/json"));

        // Add ModelBinders to ensure any stronly typed primitive types
        // used in any route or querystring parameters, are correctly bound
        // from string values to their associated primitive type values
        cfg.ModelBinderProviders.Insert(0, new PrimitivesModelBinderProvider());
    })
    .AddJsonOptionsConfiguration()
    .AddProblemDetailsConventions();
    builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);
    builder.Services.AddApiVersioningConfiguration();


    // Configure Api Documentation
    builder.Services.AddSwaggerConfiguration();

    // Configure Logging
    builder.AddSerilog();
    builder.TryAddOpenTelemetry();

    // Configure HealthChecks
    builder.Services.AddHealthChecks();

    // Configure Services
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddMassTransitMediator();
    builder.Services.ConfigureDataServices();
    builder.Services.ConfigureDomainServices();

    Log.Information("Building PSL.MicroserviceTemplate.Api...");
    var app = builder.Build();

    // Use ProblemDetails 
    app.UseProblemDetails();

    // Use CorrelationId middeware
    app.UseCorrelationId();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseRouting();

    //app.UseAuthentication();
    //app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();

        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).WithMetadata(new AllowAnonymousAttribute());
    });

    Log.Information("Starting PSL.MicroserviceTemplate.Api...");
    app.Run();
    Log.Information("Shutting down PSL.MicroserviceTemplate.Api...");
}
catch (Exception ex)
{
    Log.Fatal(ex, "PSL.MicroserviceTemplate.Api failed");
    return 1;
}
finally
{
    Log.Information("Shut down PSL.MicroserviceTemplate.Api");
    Log.CloseAndFlush();
}

return 0;
