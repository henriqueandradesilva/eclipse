using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Task.Response;

public class DeleteTaskResponse : BaseDeleteResponse
{
    public DeleteTaskResponse()
    {

    }

    public DeleteTaskResponse(
        Domain.Entities.Task task)
    {
        Id = task.Id;
    }
}