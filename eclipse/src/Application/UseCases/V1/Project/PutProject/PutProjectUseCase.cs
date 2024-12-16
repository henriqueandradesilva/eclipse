using Application.UseCases.V1.Project.PutProject.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Project.PutProject;

public class PutProjectUseCase : IPutProjectUseCase
{
    private IOutputPort<Domain.Entities.Project> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutProjectUseCase(
        IUnitOfWork unitOfWork,
        IProjectRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        Domain.Entities.Project project)
    {
        var normalizedTitle = project.Title?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != project.Id &&
                                         (
                                          c.Title.ToUpper().Trim().Contains(normalizedTitle)
                                         ))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ProjectExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == project.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.ProjectNotExist);

                _outputPort.Error();
            }
            else
            {
                if (result.Priority != project.Priority)
                {
                    _notificationHelper.Add(SystemConst.Error, MessageConst.ProjectNotChangedPriority);

                    _outputPort.Error();
                }
                else
                {
                    if (project.ExpectedStartDate >= project.ExpectedEndDate)
                    {
                        _notificationHelper.Add(SystemConst.Error, MessageConst.MessageDatetimeError);

                        _outputPort.Error();

                        return;
                    }
                    else
                    {
                        project.Map(result);

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
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Project> outputPort)
        => _outputPort = outputPort;
}