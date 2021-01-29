using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class WikiConfiguration : IEntityTypeConfiguration<Wiki>
    {
        public void Configure(EntityTypeBuilder<Wiki> builder)
        {
            builder.ToTable("Wikis");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).HasMaxLength(37);
            builder.Property(_ => _.DateCreated);
            builder.Property(_ => _.DateUpdated);
            builder.Property(_ => _.CreatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.UpdatedBy).HasMaxLength(100).HasDefaultValue("?");
            builder.Property(_ => _.ExternalId).HasMaxLength(100);
            builder.HasIndex(_ => _.ExternalId);
            builder.Property(_ => _.CustomAttributes)
                .HasColumnType("jsonb");

            builder.Property(_ => _.Title).HasMaxLength(255).IsRequired();
            builder.Property(_ => _.Body).IsRequired();
            builder.HasIndex(_ => new
            {
                _.Title,
                _.Body
            }).IsTsVectorExpressionIndex("english");
            builder.Property(_ => _.Tags).IsRequired();
            builder.HasIndex(_ => _.Tags);
        }
    }
}
