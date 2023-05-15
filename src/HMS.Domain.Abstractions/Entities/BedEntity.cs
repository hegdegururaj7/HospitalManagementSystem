namespace HMS.Service.Domain.Abstractions.Entities
{
    public class BedEntity : AuditEntity
    {
        public Guid Id { get; set; }
        public int BedNumber { get; set; }

        public bool IsOccupied { get; set; }

    }
}
