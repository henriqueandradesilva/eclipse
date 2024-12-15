using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.TaskComment.Response;

public class DeleteTaskCommentResponse : BaseDeleteResponse
{
    public DeleteTaskCommentResponse()
    {

    }

    public DeleteTaskCommentResponse(
        Domain.Entities.TaskComment taskComment)
    {
        Id = taskComment.Id;
    }
}