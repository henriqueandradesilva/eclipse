using Application.Services;
using Application.UseCases.V1.AuditLog.PostAuditLog.Interfaces;
using Application.UseCases.V1.TaskComment.PostTaskComment.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.UseCases.V1.TaskComment.PostTaskComment;

public class PostTaskCommentUseCase : IPostTaskCommentUseCase
{
    private IOutputPort<Domain.Entities.TaskComment> _outputPort;
    private IPostAuditLogUseCase _postAuditLogUseCase;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskCommentRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostTaskCommentUseCase(
        IUnitOfWork unitOfWork,
        IPostAuditLogUseCase postAuditLogUseCase,
        ITaskCommentRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _postAuditLogUseCase = postAuditLogUseCase;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.TaskComment taskComment)
    {
        var normalizedDescription = taskComment.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedDescription) &&
                                         c.TaskId == taskComment.TaskId &&
                                         c.UserId == taskComment.UserId)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.TaskCommentExist);

            _outputPort.Error();

            return;
        }

        if (taskComment.Id == 0)
        {
            var auditLog = new Domain.Entities.AuditLog(0, taskComment.TaskId, taskComment.UserId, taskComment.ToString(), DateTime.UtcNow, Domain.Common.Enums.TypeEntityEnum.Task);

            var auditLogOutputPort =
                new OutputPortService<Domain.Entities.AuditLog>(_notificationHelper);

            _postAuditLogUseCase.SetOutputPort(auditLogOutputPort);

            await _postAuditLogUseCase.Execute(auditLog);

            taskComment.SetDateCreated();

            await _repository.Add(taskComment)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();

                return;
            }

            _outputPort.Ok(taskComment);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.TaskComment> outputPort)
        => _outputPort = outputPort;
}
