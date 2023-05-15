using Microsoft.Extensions.Hosting;

namespace HMS.Service.Data.Seeder
{
    internal class SeedParametersArgumentsParser
    {
        public SeedParameters ParseArguments(List<string> args, IHostEnvironment hostEnvironment, out string testDataFilePath)
        {
            bool seedConfigData = args.Contains("/seed");
            bool resetAllData = !hostEnvironment.IsProduction() && args.Contains("/reset");
            bool flag = false;
            string text = args.FirstOrDefault((string a) => a.Contains("/testdata="));
            string[] array = text?.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (text == null || array == null || array.Length < 2)
            {
                testDataFilePath = string.Empty;
                return new SeedParameters
                {
                    ResetAllData = resetAllData,
                    SeedConfigData = seedConfigData,
                    SeedTestData = false
                };
            }

            flag = true;
            testDataFilePath = array[1];
            return new SeedParameters
            {
                ResetAllData = resetAllData,
                SeedConfigData = seedConfigData,
                SeedTestData = flag
            };
        }
    }
}
