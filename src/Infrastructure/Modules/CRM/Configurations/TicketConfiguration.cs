using Infrastructure.Modules.CRM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Modules.CRM.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);
            builder.Property(_ => _.CustomAttributes)
                .HasColumnType("jsonb");

            builder.Property(_ => _.Title).HasMaxLength(100).IsRequired();
            builder.Property(_ => _.Description);
            builder.Property(_ => _.Status)
                .HasMaxLength(100)
                .HasConversion<string>();
            builder.Property(_ => _.IsBilled);
            builder.Property(_ => _.IsPaid);

            builder.HasOne(_ => _.Customer)
                .WithMany(_ => _.Tickets)
                .HasForeignKey(_ => _.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(_ => _.ContactId);
            builder.HasIndex(_ => _.ContactId);

        }
    }
}
