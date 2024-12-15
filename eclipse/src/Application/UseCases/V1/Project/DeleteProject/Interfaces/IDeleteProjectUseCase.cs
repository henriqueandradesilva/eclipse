using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Project.DeleteProject.Interfaces;

public interface IDeleteProjectUseCase
{
    System.Threading.Tasks.Task Execute(
        long id);

    void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Project> outputPort);
}