using Application.UseCases.V1.Project.GetListAllProject.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Project.GetListAllProject;

public class GetListAllProjectUseCase : IGetListAllProjectUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Project>> _outputPort;
    private readonly IProjectRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllProjectUseCase(
        IProjectRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var query =
             _repository.GetAllWithIncludes(c => c.User,
                                                 c => c.User.UserRole);

        var result = await query?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ProjectNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Project>> outputPort)
        => _outputPort = outputPort;
}