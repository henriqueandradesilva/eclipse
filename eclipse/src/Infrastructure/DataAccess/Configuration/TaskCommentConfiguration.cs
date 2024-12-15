using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
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
        builder.Property(c => c.Description)
               .HasColumnType("varchar(150)")
               .HasMaxLength(150)
               .IsRequired();

        // Relationship One to Many
        builder.HasOne(c => c.Task)
               .WithMany(c => c.ListTaskComment)
               .HasForeignKey(c => c.TaskId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
               .WithMany(c => c.ListTaskComment)
               .HasForeignKey(c => c.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}