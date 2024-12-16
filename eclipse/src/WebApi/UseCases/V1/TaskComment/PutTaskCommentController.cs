using Application.UseCases.V1.TaskComment.PutTaskComment.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.TaskComment.Request;
using CrossCutting.Dtos.TaskComment.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.TaskComment;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas-comentarios", Name = "Tarefas Comentários")]
[ApiController]
public class PutTaskCommentController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.TaskComment>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutTaskCommentController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutTaskCommentResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um comentário de uma tarefa por id")]
    public async Task<IActionResult> PutTaskComment(
        [FromServices] IPutTaskCommentUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutTaskCommentRequest putTaskCommentRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.TaskComment taskComment =
            new Domain.Entities.TaskComment(
            id,
            putTaskCommentRequest.TarefaId,
            putTaskCommentRequest.UsuarioId,
            putTaskCommentRequest.Descricao);

        await useCase.Execute(taskComment);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.TaskComment>.Ok(
        Domain.Entities.TaskComment taskComment)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.TaskCommentUpdated);

        _viewModel = base.Ok(new GenericResponse<PutTaskCommentResponse>(true, new PutTaskCommentResponse(taskComment), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.TaskComment>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutTaskCommentResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}