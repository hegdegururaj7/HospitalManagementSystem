namespace HMS.Service.Domain.Abstractions.Interfaces
{
    public interface IMigratable
    {
        // Summary:
        //     Set the processing prioriy of a given migrator
        int Priority { get; }

        //
        // Summary:
        //     Specify the DbContext the Migrator is targeting
        Type DbContextType { get; }

        //
        // Summary:
        //     Implement any tasks required to create DB here
        //
        // Returns:
        //     Task
        Task EnsureCreated();

        //
        // Summary:
        //     Implement any tasks required to migrate DB schema here
        //
        // Returns:
        //     Task
        Task Migrate();

        //
        // Summary:
        //     Implement tasks to delete data in event of a Reset
        //
        // Returns:
        //     Task
        Task ClearData();

        //
        // Summary:
        //     Implement logic to seed any config data required for service to run
        //
        // Parameters:
        //   cancellationToken:
        //     Cancellation Token
        //
        // Returns:
        //     Task
        Task SeedConfigData(CancellationToken cancellationToken = default(CancellationToken));

        //
        // Summary:
        //     Implement logic to seed any test data you may want to insert into the db
        //
        // Parameters:
        //   cancellationToken:
        //     Cancellation Token
        //
        // Returns:
        //     Task
        Task SeedTestData(CancellationToken cancellationToken = default(CancellationToken));
    }
}
