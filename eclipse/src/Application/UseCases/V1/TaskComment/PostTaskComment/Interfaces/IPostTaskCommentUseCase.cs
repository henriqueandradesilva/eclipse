using CrossCutting.Interfaces;

namespace Application.UseCases.V1.TaskComment.PostTaskComment.Interfaces;

public interface IPostTaskCommentUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.TaskComment taskComment);

    void SetOutputPort(
        IOutputPort<Domain.Entities.TaskComment> outputPort);
}