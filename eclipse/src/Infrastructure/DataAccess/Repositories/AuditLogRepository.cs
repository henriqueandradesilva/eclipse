using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(
        EclipseDbContext context) : base(context)
    {
    }
}