using HMS.Service.Domain.Abstractions.Entities;
using HMS.Service.Domain.Abstractions.Interfaces;
using MongoDB.Driver;

namespace HMS.Service.Data
{
    public class HMSRepository : IHMSRepository
    {
        private readonly HMSContext _context;
        public HMSRepository(HMSContext context)
        {
            _context = context;
        }
        public IEnumerable<BedEntity> GetAvailableBeds()
        {
            return _context.Beds.AsQueryable().ToList();
        }

        public IEnumerable<DoctorEntity> GetAvailableDoctors()
        {
            return _context.Doctors.AsQueryable().ToList();

        }

        public PatientEntity GetByPatientName(string name)
        {
            return _context.Patients.Find(x => x.PatientName.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public bool UpdatePatientDetails(PatientEntity patientEntity)
        {
            var result = _context.Patients.UpdateOne(p => p.Id == patientEntity.Id, Builders<PatientEntity>.Update
                            .Set(g => g.DoctorId, patientEntity.DoctorId)
                            .Set(g => g.BedId, patientEntity.BedId)
                           .Set(g => g.UpdatedAt, patientEntity.UpdatedAt)
                           .Set(g => g.UpdatedBy, patientEntity.UpdatedBy));

           var bedResult =  _context.Beds.UpdateOne(p => p.Id == patientEntity.BedId, Builders<BedEntity>.Update
                            .Set(g => g.IsOccupied, false)
                           .Set(g => g.UpdatedAt, patientEntity.UpdatedAt)
                           .Set(g => g.UpdatedBy, patientEntity.UpdatedBy));

            return result.IsAcknowledged && bedResult.IsAcknowledged;
        }
    }
}
