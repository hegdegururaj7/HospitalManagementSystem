using System.Text;

namespace HMS.Service.Data.Seeder
{
    internal class SeedParameters
    {
        public bool SeedConfigData { get; init; }

        public bool ResetAllData { get; init; }

        public bool SeedTestData { get; init; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("SeedParameters {");
            stringBuilder.AppendLine(string.Format("{0}:{1}", "SeedConfigData", SeedConfigData));
            stringBuilder.AppendLine(string.Format("{0}:{1}", "ResetAllData", ResetAllData));
            stringBuilder.AppendLine(string.Format("{0}:{1}", "SeedTestData", SeedTestData));
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }
    }
}
