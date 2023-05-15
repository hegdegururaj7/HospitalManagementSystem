using HMS.Service.Domain.Abstractions.Interfaces;
using HMS.Service.Domain.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HMS.Service.Domain;

public static class DomainServicesExtensions
{
    public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICostEstimatorManager, CostEstimatorManager>();
        services.AddScoped<IIntelligentSchedulingManager, IntelligentSchedulingManager>();
        return services;
    }
}
