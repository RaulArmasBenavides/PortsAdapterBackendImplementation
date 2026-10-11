namespace CompanyName.DDD.Domain.Common.Audit
{
    /// <summary>
    /// Base for persisted entities that track who created or last changed them.
    /// Values are filled automatically by the DbContext on SaveChanges (UTC timestamps).
    /// </summary>
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
