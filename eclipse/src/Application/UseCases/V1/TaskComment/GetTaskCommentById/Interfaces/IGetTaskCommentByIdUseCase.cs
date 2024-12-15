using CrossCutting.Interfaces;

namespace Application.UseCases.V1.TaskComment.GetTaskCommentById.Interfaces;

public interface IGetTaskCommentByIdUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithForbid<Domain.Entities.TaskComment> outputPort);
}