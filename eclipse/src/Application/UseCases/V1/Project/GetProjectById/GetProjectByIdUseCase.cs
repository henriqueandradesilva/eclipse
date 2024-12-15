using Application.UseCases.V1.Project.GetProjectById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Project.GetProjectById;

public class GetProjectByIdUseCase : IGetProjectByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Project> _outputPort;
    private readonly IProjectRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetProjectByIdUseCase(
        IProjectRepository repository,
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
                             ?.Include(c => c.User)?.ThenInclude(c => c.UserRole)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ProjectNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Project> outputPort)
        => _outputPort = outputPort;
}