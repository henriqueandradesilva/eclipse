using CrossCutting.Interfaces;

namespace Application.UseCases.V1.Project.PostProject.Interfaces;

public interface IPostProjectUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.Project project);

    void SetOutputPort(
        IOutputPort<Domain.Entities.Project> outputPort);
}