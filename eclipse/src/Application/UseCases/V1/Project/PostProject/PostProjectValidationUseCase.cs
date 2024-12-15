using Application.UseCases.V1.Project.PostProject.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;

namespace Application.UseCases.V1.Project.PostProject;

public class PostProjectValidationUseCase : IPostProjectUseCase
{
    private IOutputPort<Domain.Entities.Project> _outputPort;
    private readonly IPostProjectUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostProjectValidationUseCase(
        IPostProjectUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.Project project)
    {
        if (project.Invalid())
        {
            var listNotification = project.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(project);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Project> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}