using Application.UseCases.V1.Task.PutTask.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;

namespace Application.UseCases.V1.Task.PutTask;

public class PutTaskValidationUseCase : IPutTaskUseCase
{
    private IOutputPort<Domain.Entities.Task> _outputPort;
    private readonly IPutTaskUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutTaskValidationUseCase(
        IPutTaskUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.Task task)
    {
        if (task.Invalid())
        {
            var listNotification = task.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(task);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Task> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}