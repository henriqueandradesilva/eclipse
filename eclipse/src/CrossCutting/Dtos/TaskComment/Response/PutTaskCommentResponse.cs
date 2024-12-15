using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.TaskComment.Response;

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