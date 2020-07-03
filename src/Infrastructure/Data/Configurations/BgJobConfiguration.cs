using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class BgJobConfiguration : IEntityTypeConfiguration<BgJob>
    {
        public void Configure(EntityTypeBuilder<BgJob> builder)
        {
            builder.ToTable("BgJobs");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.Name).HasMaxLength(50);
            builder.Property(_ => _.Status).HasMaxLength(50);
            builder.Property(_ => _.Data);

            builder.HasOne(_ => _.BgSchedule)
                .WithMany(_ => _.BgJobs)
                .HasForeignKey(_ => _.BgScheduleId);
        }
    }
}
