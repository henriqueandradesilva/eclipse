using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Project.Response;

public class PutProjectResponse : BaseResponse
{
    public PutProjectResponse()
    {

    }

    public PutProjectResponse(
        Domain.Entities.Project project)
    {
        Id = project.Id;
        DataCriacao = project.DateCreated;
        DataAlteracao = project.DateUpdated;
    }
}