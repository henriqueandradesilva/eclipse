using Application.UseCases.V1.TaskComment.GetListAllTaskComment.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.UseCases.V1.TaskComment.GetListAllTaskComment;

public class GetListAllTaskCommentUseCase : IGetListAllTaskCommentUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.TaskComment>> _outputPort;
    private ITaskCommentRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllTaskCommentUseCase(
        ITaskCommentRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var query =
             _repository.GetAllWithIncludes(c => c.Task,
                                                 c => c.User,
                                                 c => c.User.UserRole);

        var result = await query?.ToListAsync();

        if (result == null || result.Count == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.TaskCommentNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.TaskComment>> outputPort)
        => _outputPort = outputPort;
}