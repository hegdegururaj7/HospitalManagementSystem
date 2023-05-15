using HMS.Service.Data.Seeder;
using HMS.Service.Domain.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HMS.Service.Data
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            var seeder = SeederBuilder.Create(args)
                .AddMigrationTarget<HMSDbMigrator>()
                .RegisterServices((ctx, services) =>
                {
                    services.Configure<IMongoConfigOptions>(ctx.Configuration.GetSection("Mongo"));
                    services.AddSingleton<IMongoConfigOptions>(sp => sp.GetRequiredService<IOptions<MongoConfigOptions>>().Value);

                    services.ConfigureDataServices();
                });

            await seeder.Run();
        }
    }
}
