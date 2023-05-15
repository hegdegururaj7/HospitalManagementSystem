using HMS.Service.Domain.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace HMS.Service.Data;

public static class DataServicesExtensions
{
    public static IServiceCollection ConfigureDataServices(this IServiceCollection services)
    {
        if (!services.Any(s => s.ServiceType == typeof(IMongoConfigOptions)))
            throw new Exception();

        services.AddSingleton<IHMSMongoClientFactory, HMSMongoClientFactory>();
        services.AddScoped<HMSContext>();
        services.AddScoped<IHMSRepository, HMSRepository>();
        services.AddSingleton<IMongoClient, MongoClient>();
        return services;
    }
}
