using Application.UseCases.V1.TaskComment.PutTaskComment.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;

namespace Application.UseCases.V1.TaskComment.PutTaskComment;

public class PutTaskCommentValidationUseCase : IPutTaskCommentUseCase
{
    private IOutputPort<Domain.Entities.TaskComment> _outputPort;
    private readonly IPutTaskCommentUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutTaskCommentValidationUseCase(
        IPutTaskCommentUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.TaskComment taskComment)
    {
        if (taskComment.Invalid())
        {
            var listNotification = taskComment.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(taskComment);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.TaskComment> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}