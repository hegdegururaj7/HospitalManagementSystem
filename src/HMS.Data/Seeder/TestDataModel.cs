using HMS.Service.Domain.Abstractions.Entities;
using System.Text.Json;

namespace HMS.Service.Data.Seeder
{
    public class TestDataModel
    {
        public List<PatientEntity> Patients { get; set; } = new();
        public List<DoctorEntity> Doctors { get; set; } = new();
        public List<BedEntity> Beds { get; set; } = new();

        static readonly JsonSerializerOptions _serializerOptions = new()
        {
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        public static TestDataModel Parse(string json)
        {
            return JsonSerializer.Deserialize<TestDataModel>(json, _serializerOptions);
        }
    }
}
