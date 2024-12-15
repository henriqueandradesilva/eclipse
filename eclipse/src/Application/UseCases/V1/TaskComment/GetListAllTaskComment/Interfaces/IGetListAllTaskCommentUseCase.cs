using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.TaskComment.GetListAllTaskComment.Interfaces;

public interface IGetListAllTaskCommentUseCase
{
    System.Threading.Tasks.Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.TaskComment>> outputPort);
}