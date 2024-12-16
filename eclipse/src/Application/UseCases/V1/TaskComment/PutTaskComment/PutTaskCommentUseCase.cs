using Application.Services;
using Application.UseCases.V1.AuditLog.PostAuditLog.Interfaces;
using Application.UseCases.V1.TaskComment.PutTaskComment.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.UseCases.V1.TaskComment.PutTaskComment;

public class PutTaskCommentUseCase : IPutTaskCommentUseCase
{
    private IOutputPort<Domain.Entities.TaskComment> _outputPort;
    private IPostAuditLogUseCase _postAuditLogUseCase;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskCommentRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly NotificationHelper _notificationHelper;

    public PutTaskCommentUseCase(
        IUnitOfWork unitOfWork,
        IPostAuditLogUseCase postAuditLogUseCase,
        ITaskCommentRepository repository,
        IUserRepository userRepository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _postAuditLogUseCase = postAuditLogUseCase;
        _repository = repository;
        _userRepository = userRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.TaskComment taskComment)
    {
        var normalizedDescription = taskComment.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != taskComment.Id &&
                                         (
                                          c.Description.ToUpper().Trim().Contains(normalizedDescription) &&
                                          c.TaskId == taskComment.TaskId &&
                                          c.UserId == taskComment.UserId
                                         ))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.TaskCommentExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == taskComment.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.TaskCommentNotExist);

                _outputPort.Error();
            }
            else
            {
                taskComment.Map(result);

                result.SetDateUpdated();

                _repository.Update(result);

                var response =
                    await _unitOfWork.Save()
                                     .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(response))
                {
                    _notificationHelper.Add(SystemConst.Error, response);

                    _outputPort.Error();
                }
                else
                {
                    var user =
                        await _userRepository?.Where(c => c.Id == taskComment.UserId)
                                             ?.FirstOrDefaultAsync();

                    result.AddUser(user);

                    var auditLog = new Domain.Entities.AuditLog(0, result.TaskId, result.UserId, result.ToString(), DateTime.UtcNow, Domain.Common.Enums.TypeEntityEnum.Task);

                    var auditLogOutputPort =
                        new OutputPortService<Domain.Entities.AuditLog>(_notificationHelper);

                    _postAuditLogUseCase.SetOutputPort(auditLogOutputPort);

                    await _postAuditLogUseCase.Execute(auditLog);

                    _outputPort.Ok(result);
                }
            }
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.TaskComment> outputPort)
        => _outputPort = outputPort;
}