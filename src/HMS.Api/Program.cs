using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using HMS.Service.Api.Configuration;
using HMS.Service.Data;
using HMS.Service.Domain;
using HMS.Service.Domain.Abstractions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PSL.HMS.Service.Api.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
#if DEBUG
    .WriteTo.Debug()
#endif
    .CreateBootstrapLogger();
Log.Information("Creating HMS.Service.Api...");

try
{
    Log.Information("Configuring HMS.Service.Api...");
    var builder = WebApplication.CreateBuilder(args);

    // Configure Endpoints & Routing
    builder.AddProblemDetailsConfiguration();
    builder.Services.AddControllers(cfg =>
    {
        cfg.Filters.Add(new ProducesAttribute("application/json"));
    })
    .AddJsonOptionsConfiguration()
    .AddProblemDetailsConventions();
    builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);
    builder.Services.AddApiVersioningConfiguration();


    // Configure Api Documentation
    builder.Services.AddSwaggerConfiguration();

    // Configure HealthChecks
    builder.Services.AddHealthChecks();

    // Configure Services
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.Configure<MongoConfigOptions>(builder.Configuration.GetSection("Mongo"));
    builder.Services.AddSingleton<IMongoConfigOptions>(sp => sp.GetRequiredService<IOptions<MongoConfigOptions>>().Value);
    builder.Services.ConfigureDataServices();
    DomainServicesExtensions.ConfigureDomainServices(builder.Services);
    Log.Information("Building HMS.Service.Api...");
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


  //  app.UseHttpsRedirection();
 

    app.UseRouting();

    app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed((host) => true)
.AllowCredentials());

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

    Log.Information("Starting HMS.Service.Api...");
    app.Run();
    Log.Information("Shutting down HMS.Service.Api...");
}
catch (Exception ex)
{
    Log.Fatal(ex, "HMS.Service.Api failed");
    return 1;
}
finally
{
    Log.Information("Shut down HMS.Service.Api");
    Log.CloseAndFlush();
}

return 0;
