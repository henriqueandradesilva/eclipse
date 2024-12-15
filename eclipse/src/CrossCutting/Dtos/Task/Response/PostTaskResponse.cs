using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Task.Response;

public class PostTaskResponse : BaseResponse
{
    public PostTaskResponse()
    {

    }

    public PostTaskResponse(
        Domain.Entities.Task task)
    {
        Id = task.Id;
        DataCriacao = task.DateCreated;
        DataAlteracao = task.DateUpdated;
    }
}