namespace PSL.MicroserviceTemplate.Api.Configuration;

internal static class ConfigurationManagerExtensions
{
    /// <summary>
    /// Writes the application configuration property 
    /// Key Value Pairs to the console.
    /// </summary>
    /// <param name="configuration">The application Configuration Manager</param>
    internal static void DumpConfiguration(this IConfiguration configuration)
    {
#if DEBUG
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            Console.WriteLine($"{key} = {value}");
        }
#endif
    }
}
