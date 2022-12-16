using PSL.MicroserviceTemplate.Data;

namespace PSL.MicroserviceTemplate.Api.Configuration;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitMediator(this IServiceCollection services)
    {
        services.AddMediator(cfg =>
        {
            cfg.AddConsumers(typeof(DomainServicesExtensions).Assembly);
        });

        return services;
    }
}
