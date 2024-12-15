using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Task.Request;

public class PostTaskCommentRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.TaskIdRequired)]
    public long TarefaId { get; set; }

    [Required(ErrorMessage = MessageConst.UserIdRequired)]
    public long UsuarioId { get; set; }

    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PostTaskCommentRequest()
    {

    }

    public PostTaskCommentRequest(
        long tarefaId,
        long usuarioId,
        string descricao)
    {
        TarefaId = tarefaId;
        UsuarioId = usuarioId;
        Descricao = descricao;
    }
}