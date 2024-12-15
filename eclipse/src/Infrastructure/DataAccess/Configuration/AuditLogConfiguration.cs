using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        // Property Entity
        builder.Property(c => c.Date)
               .HasColumnType("timestamp with time zone")
               .IsRequired()
               .HasDefaultValueSql("now()");

        // Property
        builder.Property(c => c.Description)
               .HasColumnType("varchar(150)")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(c => c.TypeEntity)
               .HasColumnType("varchar(50)")
               .HasConversion<string>()
               .IsRequired();

        // Relationship One to Many
        builder.HasOne(c => c.User)
               .WithMany(c => c.ListAuditLog)
               .HasForeignKey(c => c.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}