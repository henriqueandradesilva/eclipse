using Application.UseCases.V1.Project.GetListSearchProject.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.Project.GetListSearchProject;

public class GetListSearchProjectUseCase : IGetListSearchProjectUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Project>> _outputPort;
    private readonly IProjectRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchProjectUseCase(
        IProjectRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Project>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                        x.Title.ToUpper().Trim().Contains(normalizedText) ||
                                        x.Description.ToUpper().Trim().Contains(normalizedText));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldUserId.ToUpper())
                    query = query.Where(c => c.UserId == relational.Item2);
            }
        }

        if (genericSearchPaginationRequest.ListaEnum != null &&
            genericSearchPaginationRequest.ListaEnum.Any())
        {
            foreach (var enumerator in genericSearchPaginationRequest.ListaEnum)
            {
                if (enumerator.Item1.ToUpper() == SystemConst.FieldStatus.ToUpper())
                    query = query.Where(c => c.Status == (StatusEnum)enumerator.Item2);

                if (enumerator.Item1.ToUpper() == SystemConst.FieldPriority.ToUpper())
                    query = query.Where(c => c.Priority == (PriorityEnum)enumerator.Item2);
            }
        }

        query =
            query.Where(x => (!genericSearchPaginationRequest.DataInicio.HasValue || x.ExpectedStartDate >= genericSearchPaginationRequest.DataInicio.Value ||
                                                                                     x.ExpectedEndDate >= genericSearchPaginationRequest.DataInicio) &&
                             (!genericSearchPaginationRequest.DataFim.HasValue || x.ExpectedStartDate <= genericSearchPaginationRequest.DataFim.Value ||
                                                                                  x.ExpectedEndDate <= genericSearchPaginationRequest.DataFim.Value));

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.User)?.ThenInclude(c => c.UserRole)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ProjectNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Project>> outputPort)
        => _outputPort = outputPort;
}