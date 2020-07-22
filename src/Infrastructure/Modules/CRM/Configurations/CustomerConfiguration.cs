using Infrastructure.Modules.CRM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Modules.CRM.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.Name).HasMaxLength(100).IsRequired();
            builder.HasIndex(_ => _.Name).IsUnique(true);
            builder.Property(_ => _.Address1).HasMaxLength(100);
            builder.Property(_ => _.Address2).HasMaxLength(100);
            builder.Property(_ => _.City).HasMaxLength(100);
            builder.Property(_ => _.State).HasMaxLength(50);
            builder.Property(_ => _.Zip).HasMaxLength(50);
            builder.Property(_ => _.Country).HasMaxLength(50);
            builder.Property(_ => _.Phone).HasMaxLength(50);

        }
    }
}
