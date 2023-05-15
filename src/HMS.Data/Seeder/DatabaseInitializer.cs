using HMS.Service.Domain.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace HMS.Service.Data.Seeder
{
    internal class DatabaseInitializer
    {
        private readonly List<IMigratable> _migrationTargets;

        private readonly SeedParameters _seedParameters;

        private readonly ILogger _logger;

        public DatabaseInitializer(IEnumerable<IMigratable> migrationTargets, SeedParameters seedParameters, ILogger<DatabaseInitializer> logger)
        {
            _migrationTargets = migrationTargets.OrderBy((IMigratable m) => m.Priority).ToList();
            _seedParameters = seedParameters;
            _logger = logger;
        }

        public async Task InitializeDatabase(CancellationToken cancellationToken)
        {
            await MigrateDatabases();
            await SeedData(cancellationToken);
        }

        public async Task CreateDatabase(CancellationToken cancellationToken)
        {
            await CreateDatabases();
            await SeedData(cancellationToken);
        }

        //
        // Summary:
        //     Creates the schema required in the database
        private async Task CreateDatabases()
        {
            _logger.LogInformation("Ensuring DB is created");
            try
            {
                foreach (IMigratable migrationTarget in _migrationTargets)
                {
                    await migrationTarget.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database!");
            }
        }

        private async Task MigrateDatabases()
        {
            _logger.LogInformation("Running database migrations");
            try
            {
                foreach (IMigratable migrationTarget in _migrationTargets)
                {
                    await migrationTarget.Migrate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error migrating database!");
            }
        }

        private async Task SeedData(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Seeding data");
            try
            {
                foreach (IMigratable seeder in _migrationTargets)
                {
                    if (_seedParameters.ResetAllData)
                    {
                        await seeder.ClearData();
                    }

                    if (_seedParameters.SeedConfigData)
                    {
                        await seeder.SeedConfigData(cancellationToken);
                    }

                    if (_seedParameters.SeedTestData)
                    {
                        await seeder.SeedTestData(cancellationToken);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error seeding data!");
            }
        }
    }
}
