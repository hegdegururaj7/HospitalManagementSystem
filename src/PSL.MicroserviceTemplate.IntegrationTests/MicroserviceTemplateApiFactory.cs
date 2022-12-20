using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace PSL.MicroserviceTemplate.IntegrationTests
{
    internal class MicroserviceTemplateApiFactory : WebApplicationFactory<Program>
    {
        public const string UsesWebApplicationFactory = "Uses WebApplicationFactory, RunInSerial";
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appsettings.integrationtests.json");
                config.AddJsonFile(configPath);
            });

            // Called Before the Program.cs 
            builder.ConfigureServices(services =>
            {

            });

            // Called After the Program.cs 
            builder.ConfigureTestServices(services =>
            {

            });
        }
    }
}
