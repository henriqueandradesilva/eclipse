using Application.UseCases.V1.Project.DeleteProject.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Project.DeleteProject;

public class DeleteProjectUseCase : IDeleteProjectUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Project> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _repository;
    private readonly ITaskRepository _taskRepository;
    private readonly NotificationHelper _notificationHelper;

    public DeleteProjectUseCase(
        IUnitOfWork unitOfWork,
        IProjectRepository repository,
        ITaskRepository taskRepository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _taskRepository = taskRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ProjectNotExist);

            _outputPort.NotFound();
        }
        else
        {
            var existTaskPending =
              await _taskRepository.Any(c => c.ProjectId == id &&
                                             c.Status == Domain.Common.Enums.StatusEnum.Pendente);

            if (existTaskPending)
            {
                _notificationHelper.Add(SystemConst.NotFound, MessageConst.ProjectTaskPending);

                _outputPort.NotFound();
            }
            else
            {
                _repository.Delete(result);

                var response =
                    await _unitOfWork.Save()
                                     .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(response))
                {
                    _notificationHelper.Add(SystemConst.Error, response);

                    _outputPort.Error();
                }
                else
                    _outputPort.Ok(result);
            }
        }
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Project> outputPort)
        => _outputPort = outputPort;
}