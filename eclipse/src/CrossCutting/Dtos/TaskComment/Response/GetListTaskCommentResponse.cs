using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.User.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Task.Response;

public class GetListTaskCommentResponse : BaseResponse
{
    public long TarefaId { get; set; }

    public long UsuarioId { get; set; }

    public string Descricao { get; set; }

    public string Tarefa { get; set; }

    public string Usuario { get; set; }

    public GetListTaskCommentResponse()
    {

    }

    public List<GetListTaskCommentResponse> GetListTaskComment(
        List<Domain.Entities.TaskComment> listTaskComment)
    {
        if (listTaskComment == null)
            return null;

        return listTaskComment
        .Select(e => new GetListTaskCommentResponse()
        {
            Id = e.Id,
            TarefaId = e.TaskId,
            UsuarioId = e.UserId,
            Descricao = e.Description,
            Tarefa = e.Task != null ? new GetTaskResponse().GetTask(e.Task).Titulo : null,
            Usuario = e.User != null ? new GetUserResponse().GetUser(e.User).Nome : null,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}