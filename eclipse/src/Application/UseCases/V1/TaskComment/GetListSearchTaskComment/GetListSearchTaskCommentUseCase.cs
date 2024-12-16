using Application.UseCases.V1.TaskComment.GetListSearchTaskComment.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.TaskComment.GetListSearchTaskComment;

public class GetListSearchTaskCommentUseCase : IGetListSearchTaskCommentUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.TaskComment>> _outputPort;
    private ITaskCommentRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchTaskCommentUseCase(
        ITaskCommentRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.TaskComment>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                        x.Description.ToUpper().Trim().Contains(normalizedText));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldTaskId.ToUpper())
                    query = query.Where(c => c.TaskId == relational.Item2);
                if (relational.Item1.ToUpper() == SystemConst.FieldUserId.ToUpper())
                    query = query.Where(c => c.UserId == relational.Item2);
            }
        }

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.Task)
                     ?.Include(c => c.User).ThenInclude(c => c.UserRole)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.TaskCommentNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.TaskComment>> outputPort)
        => _outputPort = outputPort;
}