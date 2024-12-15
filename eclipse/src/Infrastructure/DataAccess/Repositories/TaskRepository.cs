using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class TaskRepository : Repository<Task>, ITaskRepository
{
    public TaskRepository(
        EclipseDbContext context) : base(context)
    {
    }
}