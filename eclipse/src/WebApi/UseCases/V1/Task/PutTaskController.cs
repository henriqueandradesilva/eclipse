using Application.UseCases.V1.Task.PutTask.Interfaces;
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
public class PutTaskController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Task>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutTaskController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutTaskResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar uma tarefa por id")]
    public async Task<IActionResult> PutTask(
        [FromServices] IPutTaskUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutTaskRequest putTaskRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Task task =
            new Domain.Entities.Task(
            id,
            putTaskRequest.ProjetoId,
            putTaskRequest.UsuarioId,
            putTaskRequest.Titulo,
            putTaskRequest.Descricao,
            putTaskRequest.DataInicio,
            putTaskRequest.DataVencimento,
            putTaskRequest.Status,
            putTaskRequest.Prioridade);

        await useCase.Execute(task);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Task>.Ok(
        Domain.Entities.Task task)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.TaskUpdated);

        _viewModel = base.Ok(new GenericResponse<PutTaskResponse>(true, new PutTaskResponse(task), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Task>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutTaskResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}