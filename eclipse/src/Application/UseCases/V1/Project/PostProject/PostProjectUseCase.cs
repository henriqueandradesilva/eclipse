using Application.UseCases.V1.Project.PostProject.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.Project.PostProject;

public class PostProjectUseCase : IPostProjectUseCase
{
    private IOutputPort<Domain.Entities.Project> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostProjectUseCase(
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
           await _repository?.Where(c => c.Title.ToUpper().Trim().Contains(normalizedTitle))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ProjectExist);

            _outputPort.Error();

            return;
        }

        if (project.Id == 0)
        {
            project.SetDateCreated();

            await _repository.Add(project)
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

            _outputPort.Ok(project);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Project> outputPort)
        => _outputPort = outputPort;
}
