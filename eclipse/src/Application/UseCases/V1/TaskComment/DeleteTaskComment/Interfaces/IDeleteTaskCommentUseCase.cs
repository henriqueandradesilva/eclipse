using CrossCutting.Interfaces;

namespace Application.UseCases.V1.TaskComment.DeleteTaskComment.Interfaces;

public interface IDeleteTaskCommentUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.TaskComment> outputPort);
}