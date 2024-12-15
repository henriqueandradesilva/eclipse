using Application.UseCases.V1.Task.GetTaskById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Task.GetTaskById;

public class GetTaskByIdUseCase : IGetTaskByIdUseCase
{
    private IOutputPortWithForbid<Domain.Entities.Task> _outputPort;
    private ITaskRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetTaskByIdUseCase(
        ITaskRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.Project)
                             ?.Include(c => c.User)?.ThenInclude(c => c.UserRole)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Forbid, MessageConst.TaskNotExist);

            _outputPort.Forbid();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithForbid<Domain.Entities.Task> outputPort)
        => _outputPort = outputPort;
}