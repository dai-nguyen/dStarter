using Infrastructure.Modules.CRM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Modules.CRM.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.Title).HasMaxLength(100).IsRequired();
            builder.Property(_ => _.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(_ => _.LastName).HasMaxLength(100).IsRequired();
            builder.Property(_ => _.Email).HasMaxLength(100).IsRequired();
            builder.Property(_ => _.Phone).HasMaxLength(50);

            builder.HasOne(_ => _.Customer)
                .WithMany(_ => _.Contacts)
                .HasForeignKey(_ => _.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
