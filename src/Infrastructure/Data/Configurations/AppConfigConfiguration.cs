using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AppConfigConfiguration : IEntityTypeConfiguration<AppConfig>
    {
        public void Configure(EntityTypeBuilder<AppConfig> builder)
        {
            builder.ToTable("AppConfigs");
            builder.Property(_ => _.Id).UseHiLo();
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);
            builder.Property(_ => _.CustomAttributes)
                .HasColumnType("jsonb");

            builder.Property(_ => _.Description).HasMaxLength(255);
            builder.Property(_ => _.Key).HasMaxLength(100).IsRequired();
            builder.HasIndex(_ => _.Key).IsUnique(true);
            builder.Property(_ => _.Value);
            builder.Property(_ => _.Sensitive).IsRequired();
        }
    }
}
