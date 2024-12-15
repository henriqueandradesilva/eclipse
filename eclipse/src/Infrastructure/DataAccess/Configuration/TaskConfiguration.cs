using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        // Property Entity
        builder.Property(c => c.DateCreated)
               .HasColumnType("timestamp with time zone")
               .IsRequired()
               .HasDefaultValueSql("now()");

        builder.Property(c => c.DateUpdated)
               .HasColumnType("timestamp with time zone")
               .IsRequired(false);

        // Property
        builder.Property(c => c.Title)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Description)
               .HasColumnType("varchar(150)")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(c => c.ExpectedStartDate)
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(c => c.ExpectedEndDate)
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(c => c.Status)
               .HasColumnType("varchar(50)")
               .HasConversion<string>()
               .IsRequired();

        builder.Property(c => c.Priority)
               .HasColumnType("varchar(50)")
               .HasConversion<string>()
               .IsRequired();

        // Relationship One to Many
        builder.HasOne(c => c.Project)
               .WithMany(c => c.ListTask)
               .HasForeignKey(c => c.ProjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.User)
               .WithMany(c => c.ListTask)
               .HasForeignKey(c => c.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}