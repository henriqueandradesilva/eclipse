using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Task.PostTask.Interfaces;

public interface IPostTaskUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.Task task);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Task> outputPort);
}