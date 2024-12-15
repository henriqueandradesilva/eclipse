using Application.UseCases.V1.AuditLog.PostAuditLog.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;

namespace Application.UseCases.V1.AuditLog.PostAuditLog;

public class PostAuditLogValidationUseCase : IPostAuditLogUseCase
{
    private IOutputPort<Domain.Entities.AuditLog> _outputPort;
    private readonly IPostAuditLogUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostAuditLogValidationUseCase(
        IPostAuditLogUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.AuditLog auditLog)
    {
        try
        {
            await _useCase.Execute(auditLog);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.AuditLog> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}