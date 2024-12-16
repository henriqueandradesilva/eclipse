using Application.UseCases.V1.TaskComment.DeleteTaskComment.Interfaces;
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
public class DeleteTaskCommentController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.TaskComment>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteTaskCommentController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteTaskCommentResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um comentário de uma tarefa por id")]
    public async Task<IActionResult> DeleteTaskComment(
        [FromServices] IDeleteTaskCommentUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.TaskComment>.Ok(
        Domain.Entities.TaskComment taskComment)
        => _viewModel = base.Ok(new GenericResponse<DeleteTaskCommentResponse>(true, new DeleteTaskCommentResponse(taskComment), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.TaskComment>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteTaskCommentResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.TaskComment>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteTaskCommentResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}