using Application.UseCases.V1.TaskComment.PostTaskComment.Interfaces;
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
public class PostTaskCommentController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.TaskComment>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostTaskCommentController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostTaskCommentResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostTaskCommentResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo comentário em uma tarefa")]
    public async Task<IActionResult> PostTaskComment(
        [FromServices] IPostTaskCommentUseCase useCase,
        [FromBody][Required] PostTaskCommentRequest posTaskCommentRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.TaskComment taskComment =
            new Domain.Entities.TaskComment(
            0,
            posTaskCommentRequest.TarefaId,
            posTaskCommentRequest.UsuarioId,
            posTaskCommentRequest.Descricao);

        await useCase.Execute(taskComment);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.TaskComment>.Ok(
        Domain.Entities.TaskComment taskComment)
    {
        var uri = $"/tarefas/{taskComment.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.TaskCommentCreated);

        var response =
            new GenericResponse<PostTaskCommentResponse>(true, new PostTaskCommentResponse(taskComment), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.TaskComment>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostTaskCommentResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}