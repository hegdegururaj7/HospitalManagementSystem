using HMS.Service.Domain.Abstractions.Entities;

namespace HMS.Service.Domain.Abstractions.Interfaces
{
    public interface IHMSRepository
    {
        public PatientEntity GetByPatientName(string name);

        public IEnumerable<DoctorEntity> GetAvailableDoctors();
        public IEnumerable<BedEntity> GetAvailableBeds();
        public bool UpdatePatientDetails(PatientEntity patientEntity);


    }
}
