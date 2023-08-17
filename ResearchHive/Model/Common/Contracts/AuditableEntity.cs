namespace Model.Common.Contracts
{
    public class AuditableEntity : IAuditableEntity, ISoftDelete
    {
        
    
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedDate { get;  set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set ; }
    }
}
