using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.User.Response;
using Domain.Common.Enums;
using System;

namespace CrossCutting.Dtos.Project.Response;

public class GetProjectResponse : BaseResponse
{
    public long UsuarioId { get; set; }

    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataVencimento { get; set; }

    public StatusEnum Status { get; set; }

    public PriorityEnum Prioridade { get; set; }

    public GetUserResponse Usuario { get; set; }

    public GetProjectResponse()
    {

    }

    public GetProjectResponse GetProject(
        Domain.Entities.Project project)
    {
        if (project == null)
            return null;

        GetProjectResponse getProjectResponse = new GetProjectResponse();
        getProjectResponse.Id = project.Id;
        getProjectResponse.UsuarioId = project.UserId;
        getProjectResponse.Titulo = project.Title;
        getProjectResponse.Descricao = project.Description;
        getProjectResponse.DataInicio = project.ExpectedStartDate;
        getProjectResponse.DataVencimento = project.ExpectedEndDate;
        getProjectResponse.Status = project.Status;
        getProjectResponse.Prioridade = project.Priority;
        getProjectResponse.Usuario = project.User != null ? new GetUserResponse().GetUser(project.User) : null;
        getProjectResponse.DataCriacao = project.DateCreated;
        getProjectResponse.DataAlteracao = project.DateUpdated;

        return getProjectResponse;
    }
}