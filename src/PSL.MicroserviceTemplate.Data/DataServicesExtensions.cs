using Microsoft.Extensions.DependencyInjection;
using PSL.MicroserviceTemplate.Data.Templates;
using PSL.MicroserviceTemplate.Domain.Templates;

namespace PSL.MicroserviceTemplate.Data;

public static class DataServicesExtensions
{
    public static IServiceCollection ConfigureDataServices(this IServiceCollection services)
    {
        services.AddScoped<ITemplateRepository, TemplateRepository>();

        return services;
    }
}
