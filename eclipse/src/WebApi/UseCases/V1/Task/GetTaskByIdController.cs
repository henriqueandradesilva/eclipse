using Application.UseCases.V1.Task.GetTaskById.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Task;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefas", Name = "Tarefas")]
[ApiController]
public class GetTaskByIdController : CustomControllerBaseExtension, IOutputPortWithForbid<Domain.Entities.Task>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetTaskByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetTaskResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar uma tarefa por id")]
    public async Task<IActionResult> GetTask(
        [FromServices] IGetTaskByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithForbid<Domain.Entities.Task>.Ok(
        Domain.Entities.Task task)
        => _viewModel = base.Ok(new GenericResponse<GetTaskResponse>(true, new GetTaskResponse().GetTask(task), null, NotificationTypeEnum.Success));

    void IOutputPortWithForbid<Domain.Entities.Task>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetTaskResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithForbid<Domain.Entities.Task>.Forbid()
        => _viewModel = base.NotFound(new GenericResponse<GetTaskResponse>(true, null, _notificationHelper.Messages[SystemConst.Forbid]?.ToList(), NotificationTypeEnum.Warning));
}