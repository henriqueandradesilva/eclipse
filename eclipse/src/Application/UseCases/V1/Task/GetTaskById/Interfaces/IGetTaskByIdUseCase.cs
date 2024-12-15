using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Task.GetTaskById.Interfaces;

public interface IGetTaskByIdUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithForbid<Domain.Entities.Task> outputPort);
}