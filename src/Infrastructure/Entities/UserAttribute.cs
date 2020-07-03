namespace Infrastructure.Entities
{
    public class UserAttribute : BaseEntity
    {
        public virtual string UserId { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
    }
}
