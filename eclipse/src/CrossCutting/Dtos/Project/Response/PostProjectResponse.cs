using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Project.Response;

public class PostProjectResponse : BaseResponse
{
    public PostProjectResponse()
    {

    }

    public PostProjectResponse(
        Domain.Entities.Project project)
    {
        Id = project.Id;
        DataCriacao = project.DateCreated;
        DataAlteracao = project.DateUpdated;
    }
}