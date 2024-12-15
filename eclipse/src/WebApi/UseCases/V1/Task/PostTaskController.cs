using Application.UseCases.V1.Task.PostTask.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Task.Request;
using CrossCutting.Dtos.Task.Response;
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

namespace WebApi.UseCases.V1.Task;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas", Name = "Tarefas")]
[ApiController]
public class PostTaskController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Task>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostTaskController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostTaskResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar uma nova tarefa")]
    public async Task<IActionResult> PostTask(
        [FromServices] IPostTaskUseCase useCase,
        [FromBody][Required] PostTaskRequest posTaskRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Task task =
            new Domain.Entities.Task(
            0,
            posTaskRequest.ProjetoId,
            posTaskRequest.UsuarioId,
            posTaskRequest.Titulo,
            posTaskRequest.Descricao,
            posTaskRequest.DataInicio,
            posTaskRequest.DataVencimento,
            posTaskRequest.Status,
            posTaskRequest.Prioridade);

        await useCase.Execute(task);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Task>.Ok(
        Domain.Entities.Task task)
    {
        var uri = $"/tarefas/{task.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.TaskCreated);

        var response =
            new GenericResponse<PostTaskResponse>(true, new PostTaskResponse(task), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Task>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostTaskResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}