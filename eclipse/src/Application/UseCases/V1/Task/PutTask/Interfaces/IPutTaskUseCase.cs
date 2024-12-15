using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Task.PutTask.Interfaces;

public interface IPutTaskUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.Task task);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Task> outputPort);
}