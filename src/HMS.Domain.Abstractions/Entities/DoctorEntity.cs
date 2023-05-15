namespace HMS.Service.Domain.Abstractions.Entities
{
    public class DoctorEntity : AuditEntity
    {
        public Guid Id { get; set; }
        public string DoctorName { get; set; }

        public string Qualification { get; set; }
        public bool IsAvailable { get; set; }

    }
}
