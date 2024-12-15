using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Task.Response;

public class PutTaskCommentResponse : BaseResponse
{
    public PutTaskCommentResponse()
    {

    }

    public PutTaskCommentResponse(
        Domain.Entities.TaskComment taskComment)
    {
        Id = taskComment.Id;
        DataCriacao = taskComment.DateCreated;
        DataAlteracao = taskComment.DateUpdated;
    }
}