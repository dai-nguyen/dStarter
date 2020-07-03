using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UserAttributeConfiguration : IEntityTypeConfiguration<UserAttribute>
    {
        public void Configure(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable("UserAttributes");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.Type).HasMaxLength(50);
            builder.Property(_ => _.Value).HasMaxLength(100);
            builder.Property(_ => _.UserId).HasMaxLength(100);
            builder.HasIndex(_ => new { _.UserId, _.Type }).IsUnique(true);
        }
    }
}
