using Application.UseCases.V1.Task.GetListSearchTask.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Task.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace WebApi.UseCases.V1.Task;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas", Name = "Tarefas")]
[ApiController]
public class GetListSearchTaskController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchTaskController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListTaskResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar tarefas com paginação")]
    public IActionResult GetListSearchTask(
       [FromServices] IGetListSearchTaskUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>.Ok(
        GenericPaginationResponse<Domain.Entities.Task> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListTaskResponse>(true, new GetListTaskResponse().GetListTask(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListTaskResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListTaskResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}