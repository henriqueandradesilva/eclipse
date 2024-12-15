using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Project.Response;
using CrossCutting.Dtos.User.Response;
using Domain.Common.Enums;
using System;

namespace CrossCutting.Dtos.Task.Response;

public class GetTaskResponse : BaseResponse
{
    public long ProjetoId { get; set; }

    public long UsuarioId { get; set; }

    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataVencimento { get; set; }

    public StatusEnum Status { get; set; }

    public PriorityEnum Prioridade { get; set; }

    public GetProjectResponse Projeto { get; set; }

    public GetUserResponse Usuario { get; set; }

    public GetTaskResponse()
    {

    }

    public GetTaskResponse GetTask(
        Domain.Entities.Task task)
    {
        if (task == null)
            return null;

        GetTaskResponse getTaskResponse = new GetTaskResponse();
        getTaskResponse.Id = task.Id;
        getTaskResponse.ProjetoId = task.ProjectId;
        getTaskResponse.UsuarioId = task.UserId;
        getTaskResponse.Titulo = task.Title;
        getTaskResponse.Descricao = task.Description;
        getTaskResponse.DataInicio = task.ExpectedStartDate;
        getTaskResponse.DataVencimento = task.ExpectedEndDate;
        getTaskResponse.Status = task.Status;
        getTaskResponse.Prioridade = task.Priority;
        getTaskResponse.Projeto = task.Project != null ? new GetProjectResponse().GetProject(task.Project) : null;
        getTaskResponse.Usuario = task.User != null ? new GetUserResponse().GetUser(task.User) : null;
        getTaskResponse.DataCriacao = task.DateCreated;
        getTaskResponse.DataAlteracao = task.DateUpdated;

        return getTaskResponse;
    }
}