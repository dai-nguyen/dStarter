namespace Infrastructure.Entities
{
    public class RoleAttribute : BaseEntity
    {
        public virtual string RoleId { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
    }
}
