using CrossCutting.Interfaces;

namespace Application.UseCases.V1.TaskComment.PutTaskComment.Interfaces;

public interface IPutTaskCommentUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.TaskComment taskComment);

    void SetOutputPort(
        IOutputPort<Domain.Entities.TaskComment> outputPort);
}