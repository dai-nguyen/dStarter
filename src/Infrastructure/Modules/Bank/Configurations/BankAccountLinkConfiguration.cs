using Infrastructure.Modules.Bank.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Modules.Bank.Configurations
{
    public class BankAccountLinkConfiguration : IEntityTypeConfiguration<BankAccountLink>
    {
        public void Configure(EntityTypeBuilder<BankAccountLink> builder)
        {
            builder.ToTable("BankAccountLinks");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);
            builder.Property(_ => _.CustomAttributes)
                .HasColumnType("jsonb");

            builder.Property(_ => _.Name).HasMaxLength(100).IsRequired();
            builder.Property(_ => _.UserId).HasMaxLength(100).IsRequired();

            builder.HasOne(_ => _.BankAccount)
                .WithMany(_ => _.Links)
                .HasForeignKey(_ => _.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
