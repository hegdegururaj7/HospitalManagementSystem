namespace HMS.Service.Domain.Abstractions.Entities
{
    public abstract class AuditEntity
    {
        /// <summary>
        /// Created At
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Created By
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Updated At
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Updated By
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
