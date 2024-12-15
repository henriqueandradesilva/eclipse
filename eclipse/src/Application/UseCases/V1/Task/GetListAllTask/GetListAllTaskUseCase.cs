using Application.UseCases.V1.Task.GetListAllTask.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.UseCases.V1.Task.GetListAllTask;

public class GetListAllTaskUseCase : IGetListAllTaskUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Task>> _outputPort;
    private ITaskRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllTaskUseCase(
        ITaskRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var query =
             _repository.GetAllWithIncludes(c => c.Project,
                                                 c => c.User,
                                                 c => c.User.UserRole);

        var result = await query?.ToListAsync();

        if (result == null || result.Count == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.TaskNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Task>> outputPort)
        => _outputPort = outputPort;
}