using Application.UseCases.V1.Task.PutTask.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Task.PutTask;

public class PutTaskUseCase : IPutTaskUseCase
{
    private IOutputPort<Domain.Entities.Task> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutTaskUseCase(
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
           await _repository?.Where(c => c.Id != task.Id &&
                                         (
                                          c.Title.ToUpper().Trim().Contains(normalizedTitle) &&
                                          c.ProjectId == task.ProjectId
                                         ))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.TaskExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == task.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.TaskNotExist);

                _outputPort.Error();
            }
            else
            {
                if (result.Priority != task.Priority)
                {
                    _notificationHelper.Add(SystemConst.Error, MessageConst.TaskNotChangedPriority);

                    _outputPort.Error();
                }
                else
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

                    task.Map(result);

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
                        _outputPort.Ok(result);
                }
            }
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Task> outputPort)
        => _outputPort = outputPort;
}