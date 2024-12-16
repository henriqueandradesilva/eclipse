using Application.UseCases.V1.TaskComment.GetListSearchTaskComment.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.TaskComment.Response;
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

namespace WebApi.UseCases.V1.TaskComment;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas-comentarios", Name = "Tarefas Comentários")]
[ApiController]
public class GetListSearchTaskCommentController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.TaskComment>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchTaskCommentController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListTaskCommentResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar comentários das tarefas com paginação")]
    public IActionResult GetListSearchTaskComment(
       [FromServices] IGetListSearchTaskCommentUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.TaskComment>>.Ok(
        GenericPaginationResponse<Domain.Entities.TaskComment> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListTaskCommentResponse>(true, new GetListTaskCommentResponse().GetListTaskComment(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.TaskComment>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListTaskCommentResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.TaskComment>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListTaskCommentResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}