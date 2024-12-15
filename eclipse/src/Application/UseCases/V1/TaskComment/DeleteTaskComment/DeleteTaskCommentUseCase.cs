using Application.UseCases.V1.TaskComment.DeleteTaskComment.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.V1.TaskComment.DeleteTaskComment;

public class DeleteTaskCommentUseCase : IDeleteTaskCommentUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.TaskComment> _outputPort;
    private IUnitOfWork _unitOfWork;
    private ITaskCommentRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public DeleteTaskCommentUseCase(
        IUnitOfWork unitOfWork,
        ITaskCommentRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
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
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.TaskCommentNotExist);

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

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.TaskComment> outputPort)
        => _outputPort = outputPort;
}