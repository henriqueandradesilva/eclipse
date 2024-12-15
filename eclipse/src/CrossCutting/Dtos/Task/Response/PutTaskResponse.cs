using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Task.Response;

public class PutTaskResponse : BaseResponse
{
    public PutTaskResponse()
    {

    }

    public PutTaskResponse(
        Domain.Entities.Task task)
    {
        Id = task.Id;
        DataCriacao = task.DateCreated;
        DataAlteracao = task.DateUpdated;
    }
}