using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class BgScheduleConfiguration : IEntityTypeConfiguration<BgSchedule>
    {
        public void Configure(EntityTypeBuilder<BgSchedule> builder)
        {
            builder.ToTable("BgSchedules");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.Name).HasMaxLength(50);
            builder.HasIndex(_ => _.Name).IsUnique(true);
            builder.Property(_ => _.Description).HasMaxLength(255);
            builder.Property(_ => _.Enabled);
            builder.Property(_ => _.Status).HasMaxLength(50);
            builder.Property(_ => _.Monday);
            builder.Property(_ => _.Tuesday);
            builder.Property(_ => _.Wednesday);
            builder.Property(_ => _.Thursday);
            builder.Property(_ => _.Friday);
            builder.Property(_ => _.Saturday);
            builder.Property(_ => _.Sunday);
            builder.Property(_ => _.StartHour);
            builder.Property(_ => _.StartMinute);
            builder.Property(_ => _.RepeatInMinute);
        }
    }
}
