using Application.UseCases.V1.TaskComment.GetListAllTaskComment.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.TaskComment;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas-comentarios", Name = "Tarefas Comentários")]
[ApiController]
public class GetListAllTaskCommentController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.TaskComment>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllTaskCommentController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListTaskCommentResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListTaskCommentResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListTaskCommentResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos comentários das tarefas")]
    public async Task<IActionResult> GetListTaskComment(
        [FromServices] IGetListAllTaskCommentUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.TaskComment>>.Ok(
        List<Domain.Entities.TaskComment> listTaskComment)
        => _viewModel = base.Ok(new GenericResponse<List<GetListTaskCommentResponse>>(true, new GetListTaskCommentResponse().GetListTaskComment(listTaskComment), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.TaskComment>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListTaskCommentResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.TaskComment>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListTaskCommentResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}