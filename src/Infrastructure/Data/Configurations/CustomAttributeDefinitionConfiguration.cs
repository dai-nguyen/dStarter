using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CustomAttributeDefinitionConfiguration : IEntityTypeConfiguration<CustomAttributeDefinition>
    {
        public void Configure(EntityTypeBuilder<CustomAttributeDefinition> builder)
        {
            builder.ToTable("CustomAttributes");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.ObjectName).HasMaxLength(100);
            builder.HasIndex(_ => _.ObjectName);
            builder.Property(_ => _.Name).HasMaxLength(100);
            builder.Property(_ => _.DisplayName).HasMaxLength(100);
            builder.Property(_ => _.DataType).HasMaxLength(100);
        }
    }
}
