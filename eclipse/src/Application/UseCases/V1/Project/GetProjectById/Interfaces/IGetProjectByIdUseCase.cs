using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Project.GetProjectById.Interfaces;

public interface IGetProjectByIdUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Project> outputPort);
}