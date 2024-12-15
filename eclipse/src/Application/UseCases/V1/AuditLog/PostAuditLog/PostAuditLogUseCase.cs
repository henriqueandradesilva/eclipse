using Application.UseCases.V1.AuditLog.PostAuditLog.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;

namespace Application.UseCases.V1.AuditLog.PostAuditLog;

public class PostAuditLogUseCase : IPostAuditLogUseCase
{
    private IOutputPort<Domain.Entities.AuditLog> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostAuditLogUseCase(
        IUnitOfWork unitOfWork,
        IAuditLogRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.AuditLog auditLog)
    {
        if (auditLog.Id == 0)
        {
            await _repository.Add(auditLog)
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

            _outputPort.Ok(auditLog);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.AuditLog> outputPort)
        => _outputPort = outputPort;
}
