using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.DataAccess.Repositories;

public class TaskCommentRepository : Repository<TaskComment>, ITaskCommentRepository
{
    public TaskCommentRepository(
        EclipseDbContext context) : base(context)
    {
    }
}