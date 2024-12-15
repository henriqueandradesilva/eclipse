using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Task.Response;

public class PostTaskCommentResponse : BaseResponse
{
    public PostTaskCommentResponse()
    {

    }

    public PostTaskCommentResponse(
        Domain.Entities.TaskComment taskComment)
    {
        Id = taskComment.Id;
        DataCriacao = taskComment.DateCreated;
        DataAlteracao = taskComment.DateUpdated;
    }
}