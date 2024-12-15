using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Task.GetListAllTask.Interfaces;

public interface IGetListAllTaskUseCase
{
    System.Threading.Tasks.Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Task>> outputPort);
}