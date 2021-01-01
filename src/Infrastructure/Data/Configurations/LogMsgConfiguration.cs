using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class LogMsgConfiguration : IEntityTypeConfiguration<LogMsg>
    {
        public void Configure(EntityTypeBuilder<LogMsg> builder)
        {
            builder.ToTable("Logs");
            builder.Property(_ => _.message);
            builder.Property(_ => _.message_template);
            builder.Property(_ => _.level).HasMaxLength(50);
            builder.HasIndex(_ => _.level);
            builder.Property(_ => _.raise_date);
            builder.Property(_ => _.exception);
            builder.Property(_ => _.properties)
                .HasColumnType("jsonb");
            builder.Property(_ => _.props_test)
                .HasColumnType("jsonb");
            builder.Property(_ => _.machine_name);
            builder.Property(_ => _.user_name);
            builder.HasIndex(_ => _.user_name);
            builder.HasIndex(_ => new 
            { 
                _.message, 
                _.exception 
            }).IsTsVectorExpressionIndex("english");
            builder.HasNoKey();
        }
    }
}
