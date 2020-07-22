using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CustomAttributeConfiguration : IEntityTypeConfiguration<CustomAttribute>
    {
        public void Configure(EntityTypeBuilder<CustomAttribute> builder)
        {
            builder.ToTable("CustomAttributes");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);

            builder.Property(_ => _.Value);

            builder.HasOne(_ => _.Definition)
                .WithMany(_ => _.CustomAttributes)
                .HasForeignKey(_ => _.DefinitionId);
        }
    }
}
