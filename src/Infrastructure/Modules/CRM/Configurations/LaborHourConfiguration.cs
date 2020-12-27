using Infrastructure.Modules.CRM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Modules.CRM.Configurations
{
    public class LaborHourConfiguration : IEntityTypeConfiguration<LaborHour>
    {
        public void Configure(EntityTypeBuilder<LaborHour> builder)
        {
            builder.ToTable("LaborHours");
            builder.Property(_ => _.Id).UseHiLo();
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);
            builder.Property(_ => _.CustomAttributes)
                .HasColumnType("jsonb");

            builder.Property(_ => _.Hour).IsRequired();
            builder.Property(_ => _.Minute).IsRequired();
            builder.Property(_ => _.Description).IsRequired();



            builder.HasOne(_ => _.Ticket)
                .WithMany(_ => _.LaborHours)
                .HasForeignKey(_ => _.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
