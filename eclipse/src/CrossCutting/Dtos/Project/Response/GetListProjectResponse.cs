using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.User.Response;
using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Project.Response;

public class GetListProjectResponse : BaseResponse
{
    public long UsuarioId { get; set; }

    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataVencimento { get; set; }

    public StatusEnum Status { get; set; }

    public PriorityEnum Prioridade { get; set; }

    public string Usuario { get; set; }

    public GetListProjectResponse()
    {

    }

    public List<GetListProjectResponse> GetListProject(
        List<Domain.Entities.Project> listProject)
    {
        if (listProject == null)
            return null;

        return listProject
        .Select(e => new GetListProjectResponse()
        {
            Id = e.Id,
            UsuarioId = e.UserId,
            Titulo = e.Title,
            Descricao = e.Description,
            DataInicio = e.ExpectedStartDate,
            DataVencimento = e.ExpectedEndDate,
            Status = e.Status,
            Prioridade = e.Priority,
            Usuario = e.User != null ? new GetUserResponse().GetUser(e.User).Nome : null,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}