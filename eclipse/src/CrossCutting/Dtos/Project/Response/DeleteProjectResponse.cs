using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Project.Response;

public class DeleteProjectResponse : BaseDeleteResponse
{
    public DeleteProjectResponse()
    {

    }

    public DeleteProjectResponse(
        Domain.Entities.Project project)
    {
        Id = project.Id;
    }
}