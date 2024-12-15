using Application.UseCases.V1.Task.DeleteTask.Interfaces;
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
public class DeleteTaskController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Task>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteTaskController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteTaskResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteTaskResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um tarefa por id")]
    public async Task<IActionResult> DeleteTask(
        [FromServices] IDeleteTaskUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Task>.Ok(
        Domain.Entities.Task task)
        => _viewModel = base.Ok(new GenericResponse<DeleteTaskResponse>(true, new DeleteTaskResponse(task), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Task>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteTaskResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Task>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteTaskResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}