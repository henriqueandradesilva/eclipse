using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.User.Response;

namespace CrossCutting.Dtos.Task.Response;

public class GetTaskCommentResponse : BaseResponse
{
    public long TarefaId { get; set; }

    public long UsuarioId { get; set; }

    public string Descricao { get; set; }

    public GetTaskResponse Tarefa { get; set; }

    public GetUserResponse Usuario { get; set; }

    public GetTaskCommentResponse()
    {

    }

    public GetTaskCommentResponse GetTaskComment(
        Domain.Entities.TaskComment taskComment)
    {
        if (taskComment == null)
            return null;

        GetTaskCommentResponse getTaskCommentResponse = new GetTaskCommentResponse();
        getTaskCommentResponse.Id = taskComment.Id;
        getTaskCommentResponse.TarefaId = taskComment.TaskId;
        getTaskCommentResponse.UsuarioId = taskComment.UserId;
        getTaskCommentResponse.Descricao = taskComment.Description;
        getTaskCommentResponse.Tarefa = taskComment.Task != null ? new GetTaskResponse().GetTask(taskComment.Task) : null;
        getTaskCommentResponse.Usuario = taskComment.User != null ? new GetUserResponse().GetUser(taskComment.User) : null;
        getTaskCommentResponse.DataCriacao = taskComment.DateCreated;
        getTaskCommentResponse.DataAlteracao = taskComment.DateUpdated;

        return getTaskCommentResponse;
    }
}