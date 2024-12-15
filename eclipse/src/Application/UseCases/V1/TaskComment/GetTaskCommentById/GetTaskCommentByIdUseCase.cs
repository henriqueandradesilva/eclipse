using Application.UseCases.V1.TaskComment.GetTaskCommentById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.TaskComment.GetTaskCommentById;

public class GetTaskCommentByIdUseCase : IGetTaskCommentByIdUseCase
{
    private IOutputPortWithForbid<Domain.Entities.TaskComment> _outputPort;
    private ITaskCommentRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetTaskCommentByIdUseCase(
        ITaskCommentRepository repository,
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
                             ?.Include(c => c.Task)
                             ?.Include(c => c.User)?.ThenInclude(c => c.UserRole)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Forbid, MessageConst.TaskCommentNotExist);

            _outputPort.Forbid();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithForbid<Domain.Entities.TaskComment> outputPort)
        => _outputPort = outputPort;
}