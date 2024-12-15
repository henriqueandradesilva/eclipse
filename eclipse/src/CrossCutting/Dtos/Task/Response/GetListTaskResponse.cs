using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Project.Response;
using CrossCutting.Dtos.User.Response;
using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Task.Response;

public class GetListTaskResponse : BaseResponse
{
    public long ProjetoId { get; set; }

    public long UsuarioId { get; set; }

    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataVencimento { get; set; }

    public StatusEnum Status { get; set; }

    public PriorityEnum Prioridade { get; set; }

    public string Projeto { get; set; }

    public string Usuario { get; set; }

    public GetListTaskResponse()
    {

    }

    public List<GetListTaskResponse> GetListTask(
        List<Domain.Entities.Task> listTask)
    {
        if (listTask == null)
            return null;

        return listTask
        .Select(e => new GetListTaskResponse()
        {
            Id = e.Id,
            ProjetoId = e.ProjectId,
            UsuarioId = e.UserId,
            Titulo = e.Title,
            Descricao = e.Description,
            DataInicio = e.ExpectedStartDate,
            DataVencimento = e.ExpectedEndDate,
            Status = e.Status,
            Prioridade = e.Priority,
            Projeto = e.Project != null ? new GetProjectResponse().GetProject(e.Project).Titulo : null,
            Usuario = e.User != null ? new GetUserResponse().GetUser(e.User).Nome : null,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}