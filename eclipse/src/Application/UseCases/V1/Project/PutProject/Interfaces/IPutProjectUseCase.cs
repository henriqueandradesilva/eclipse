using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Project.PutProject.Interfaces;

public interface IPutProjectUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.Project project);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Project> outputPort);
}