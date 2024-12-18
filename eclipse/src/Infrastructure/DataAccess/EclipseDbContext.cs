using Infrastructure.DataAccess.Seeds;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess;

public sealed class EclipseDbContext : DbContext
{
    public EclipseDbContext(
        DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EclipseDbContext).Assembly);

        SeedData.Seed(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        //AuditLog();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AuditLog()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.OriginalValues.Properties.Any(p => p.Name == "DateUpdated"))
            {
                var dateUpdated = entry.CurrentValues["DateUpdated"];

                if (dateUpdated != null && entry.State == EntityState.Modified)
                    Log.Debug($"Entity: {entry.Entity.GetType().Name}, Date Updated after set: {dateUpdated}");
            }

            if (entry.OriginalValues.Properties.Any(p => p.Name == "DateCreated"))
            {
                var dateCreated = entry.CurrentValues["DateCreated"];

                if (dateCreated != null && entry.State == EntityState.Added)
                    Log.Debug($"Entity: {entry.Entity.GetType().Name}, Date Created set: {dateCreated}");
            }

            if (entry.State == EntityState.Deleted)
            {
                var entityType = entry.Entity.GetType().Name;
                var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Id")?.CurrentValue;

                if (entityId != null)
                    Log.Debug($"Entity: {entityType}, Id: {entityId}");
            }
        }
    }
}