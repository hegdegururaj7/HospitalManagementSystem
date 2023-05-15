using HMS.Service.Domain.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HMS.Service.Data.Seeder
{
    public class SeederBuilder
    {
        private readonly string[] _inputArgs;

        private readonly ConfigurationConsoleWriter _configurationConsoleWriter;

        private readonly SeedParametersArgumentsParser _seedParametersArgumentsParser;

        private List<Type> _migrationTargetTypes = new List<Type>();

        private Action<HostBuilderContext, IServiceCollection> _registerServicesAction;

        private IHost _host;

        private SeederBuilder(string[] inputArgs)
        {
            _inputArgs = inputArgs;
            _configurationConsoleWriter = new ConfigurationConsoleWriter();
            _seedParametersArgumentsParser = new SeedParametersArgumentsParser();
        }

        //
        // Summary:
        //     Create instsance of PSL.Data.Seeder.SeederBuilder
        //
        // Parameters:
        //   args:
        //     Input params
        //
        // Returns:
        //     PSL.Data.Seeder.SeederBuilder
        public static SeederBuilder Create(string[] args)
        {
            return new SeederBuilder(args);
        }

        //
        // Summary:
        //     Register Migrators that handle migrations and seeding for a specific database
        //
        // Type parameters:
        //   T:
        //     PSL.Data.Seeder.IMigratable
        //
        // Returns:
        //     Seeder Builder Instance
        public SeederBuilder AddMigrationTarget<T>() where T : class, IMigratable
        {
            _migrationTargetTypes.Add(typeof(T));
            return this;
        }

        //
        // Summary:
        //     Register additional services that may be required by Migrators
        //
        // Parameters:
        //   registerServicesAction:
        public SeederBuilder RegisterServices(Action<HostBuilderContext, IServiceCollection> registerServicesAction)
        {
            _registerServicesAction = registerServicesAction;
            return this;
        }

        //
        // Summary:
        //     Invoke configured Seeder
        //
        // Parameters:
        //   cancellationToken:
        public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_migrationTargetTypes.Any())
            {
                _host = CreateHostBuilder(_inputArgs).Build();
                Console.WriteLine("Running Data Seeder");
                using (IServiceScope scope = _host.Services.GetService<IServiceScopeFactory>()!.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<DatabaseInitializer>().InitializeDatabase(cancellationToken);
                }

                Console.WriteLine("Data Seeder complete");
                await _host.StopAsync();
            }
        }

        private IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(delegate (HostBuilderContext ctx, IServiceCollection services)
            {
                Console.WriteLine("Initializing Data Seeder");
                _configurationConsoleWriter.WriteConfigurationValuesToConsole(args, ctx);
                services.AddOptions();
                string testDataFilePath;
                SeedParameters seedParameters = _seedParametersArgumentsParser.ParseArguments(args.ToList(), ctx.HostingEnvironment, out testDataFilePath);
                Console.WriteLine(seedParameters);
                Console.WriteLine("testDataFilePath: " + testDataFilePath);
                services.AddSingleton(seedParameters);
                RegisterTestFile(services, testDataFilePath);
                if (_registerServicesAction != null)
                {
                    _registerServicesAction(ctx, services);
                }

                foreach (Type migrationTargetType in _migrationTargetTypes)
                {
                    services.AddTransient(typeof(IMigratable), migrationTargetType);
                    Console.WriteLine("MigrationTarget Registered: " + migrationTargetType.Name);
                }

                services.AddTransient<DatabaseInitializer>();
            }).ConfigureLogging(delegate (HostBuilderContext ctx, ILoggingBuilder loggger)
            {
                loggger.AddConsole();
            });
        }

        private void RegisterTestFile(IServiceCollection services, string testDataFilePath)
        {
            if (!File.Exists(testDataFilePath))
            {
                Console.WriteLine("TestDataFile NOT found at " + testDataFilePath);
                services.AddSingleton((Func<IServiceProvider, TestDataFileContents>)((IServiceProvider sp) => null));
            }
            else
            {
                TestDataFileContents implementationInstance = new TestDataFileContents
                {
                    Contents = File.ReadAllText(testDataFilePath)
                };
                Console.WriteLine("TestDataFile found at " + testDataFilePath);
                services.AddSingleton(implementationInstance);
            }
        }
    }
}
