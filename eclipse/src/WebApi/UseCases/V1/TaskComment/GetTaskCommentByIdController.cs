using Application.UseCases.V1.TaskComment.GetTaskCommentById.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.TaskComment;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas-comentarios", Name = "Tarefas Comentários")]
[ApiController]
public class GetTaskCommentByIdController : CustomControllerBaseExtension, IOutputPortWithForbid<Domain.Entities.TaskComment>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetTaskCommentByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetTaskCommentResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um comentário de uma tarefa por id")]
    public async Task<IActionResult> GetTaskComment(
        [FromServices] IGetTaskCommentByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithForbid<Domain.Entities.TaskComment>.Ok(
        Domain.Entities.TaskComment taskComment)
        => _viewModel = base.Ok(new GenericResponse<GetTaskCommentResponse>(true, new GetTaskCommentResponse().GetTaskComment(taskComment), null, NotificationTypeEnum.Success));

    void IOutputPortWithForbid<Domain.Entities.TaskComment>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetTaskCommentResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithForbid<Domain.Entities.TaskComment>.Forbid()
        => _viewModel = base.NotFound(new GenericResponse<GetTaskCommentResponse>(true, null, _notificationHelper.Messages[SystemConst.Forbid]?.ToList(), NotificationTypeEnum.Warning));
}