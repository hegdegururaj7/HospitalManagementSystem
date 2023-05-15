using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace HMS.Service.Data.Seeder
{
    internal class ConfigurationConsoleWriter
    {
        private readonly string[] _configKeysToLog = new string[0];

        private readonly string[] _secureConfigKeysToLog = new string[2] { "ConnectionStrings", "Mongo" };

        public Task WriteConfigurationValuesToConsole(string[] args, HostBuilderContext ctx)
        {
            Console.WriteLine("Startup Args: " + string.Join(' ', args));
            Console.WriteLine("HostEnvironmentName: " + ctx.HostingEnvironment.EnvironmentName);
            ConfigurationRoot root = ctx.Configuration as ConfigurationRoot;
            StringBuilder stringBuilder2 = new StringBuilder();
            List<string> keysToLog = _configKeysToLog.ToList();
            if (!ctx.HostingEnvironment.IsProduction())
            {
                keysToLog.AddRange(_secureConfigKeysToLog);
            }

            RecurseChildren(stringBuilder2, from c in root.GetChildren()
                                            where keysToLog.Any((string k) => k == c.Key)
                                            select c, "");
            Console.WriteLine(stringBuilder2.ToString());
            return Task.CompletedTask;
            void RecurseChildren(StringBuilder stringBuilder, IEnumerable<IConfigurationSection> children, string indent)
            {
                foreach (IConfigurationSection child in children)
                {
                    (string, IConfigurationProvider) valueAndProvider = GetValueAndProvider(root, child.Path);
                    if (valueAndProvider.Item2 != null)
                    {
                        stringBuilder.Append(indent).Append(child.Key).Append('=')
                            .Append(valueAndProvider.Item1)
                            .Append(" (")
                            .Append(valueAndProvider.Item2)
                            .AppendLine(")");
                    }
                    else
                    {
                        stringBuilder.Append(indent).Append(child.Key).AppendLine(":");
                    }

                    RecurseChildren(stringBuilder, child.GetChildren(), indent + "  ");
                }
            }
        }

        private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(IConfigurationRoot root, string key)
        {
            foreach (IConfigurationProvider item in root.Providers.Reverse())
            {
                if (item.TryGet(key, out var value))
                {
                    return (value, item);
                }
            }

            return (null, null);
        }
    }
}
