namespace HMS.Service.Domain.Abstractions.Entities
{
    public class PatientEntity : AuditEntity
    {
        public Guid Id { get; set; }
        public string PatientName { get; set; }

        public Guid DoctorId { get; set; }
        public Guid BedId { get; set; }
    }
}
