using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using Domain.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Task.Request;

public class PostTaskRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.ProjectIdRequired)]
    public long ProjetoId { get; set; }

    [Required(ErrorMessage = MessageConst.UserIdRequired)]
    public long UsuarioId { get; set; }

    [Required(ErrorMessage = MessageConst.TitleRequired)]
    public string Titulo { get; set; }

    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    [Required(ErrorMessage = MessageConst.ExpectedStartDateRequired)]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = MessageConst.ExpectedEndDateRequired)]
    public DateTime DataVencimento { get; set; }

    [Required(ErrorMessage = MessageConst.StatusRequired)]
    public StatusEnum Status { get; set; }

    [Required(ErrorMessage = MessageConst.PriorityRequired)]
    public PriorityEnum Prioridade { get; set; }

    public PostTaskRequest()
    {

    }

    public PostTaskRequest(
        long projetoId,
        long usuarioId,
        string titulo,
        string descricao,
        DateTime dataInicio,
        DateTime dataVencimento,
        StatusEnum status,
        PriorityEnum priority)
    {
        ProjetoId = projetoId;
        UsuarioId = usuarioId;
        Titulo = titulo;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataVencimento = dataVencimento;
        Status = status;
        Prioridade = priority;
    }
}