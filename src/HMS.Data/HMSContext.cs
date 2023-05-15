using HMS.Service.Domain.Abstractions.Entities;
using MongoDB.Driver;

namespace HMS.Service.Data
{
    public class HMSContext
    {
        public static string DatabaseName { get; } = "hmsdb";
        public static string PatientCollectionName { get; } = "patients";
        public static string DoctorCollectionName { get; } = "doctors";
        public static string BedCollectionName { get; } = "beds";


        public HMSContext(IHMSMongoClientFactory hMSMongoClientFactory)
        {
            var client = hMSMongoClientFactory.Create();
            var database = client.GetDatabase(DatabaseName);

            Patients = database.GetCollection<PatientEntity>(PatientCollectionName);
            Doctors = database.GetCollection<DoctorEntity>(DoctorCollectionName);
            Beds = database.GetCollection<BedEntity>(BedCollectionName);

        }

        public virtual IMongoCollection<PatientEntity> Patients { get; init; }
        public virtual IMongoCollection<DoctorEntity> Doctors { get; init; }
        public virtual IMongoCollection<BedEntity> Beds { get; init; }


    }
}
