using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Project.GetListAllProject.Interfaces;

public interface IGetListAllProjectUseCase
{
    System.Threading.Tasks.Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Project>> outputPort);
}