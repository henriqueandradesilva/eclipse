using Application.UseCases.V1.Task.PostTask.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Task.PostTask;

public class PostTaskUseCase : IPostTaskUseCase
{
    private IOutputPort<Domain.Entities.Task> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostTaskUseCase(
        IUnitOfWork unitOfWork,
        ITaskRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.Task task)
    {
        var normalizedTitle = task.Title?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Title.ToUpper().Trim().Contains(normalizedTitle) &&
                                         c.ProjectId == task.Id)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.TaskExist);

            _outputPort.Error();

            return;
        }

        if (task.Id == 0)
        {
            var countTask =
                await _repository?.Where(c => c.ProjectId == task.ProjectId)
                                 ?.CountAsync();

            if (countTask >= SystemConst.TaskMaxPermitted)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.TaskMaxPermitted);

                _outputPort.Error();

                return;
            }

            task.SetDateCreated();

            await _repository.Add(task)
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

            _outputPort.Ok(task);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Task> outputPort)
        => _outputPort = outputPort;
}
