using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Task.DeleteTask.Interfaces;

public interface IDeleteTaskUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Task> outputPort);
}