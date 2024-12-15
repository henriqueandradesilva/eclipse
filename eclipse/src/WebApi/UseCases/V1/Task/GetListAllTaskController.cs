using Application.UseCases.V1.Task.GetListAllTask.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Task;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tarefa", Name = "Tarefas")]
[ApiController]
public class GetListAllTaskController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Task>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllTaskController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListTaskResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListTaskResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListTaskResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todas as tarefas")]
    public async Task<IActionResult> GetListTask(
        [FromServices] IGetListAllTaskUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Task>>.Ok(
        List<Domain.Entities.Task> listTask)
        => _viewModel = base.Ok(new GenericResponse<List<GetListTaskResponse>>(true, new GetListTaskResponse().GetListTask(listTask), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Task>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListTaskResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Task>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListTaskResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}