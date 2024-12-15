using Application.UseCases.V1.Reports.GetListTaskByPriorityReport.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Reports.Response;
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

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/relatorios", Name = "Relatorios")]
[ApiController]
public class TaskByPriorityReportController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public TaskByPriorityReportController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("tarefas-por-prioridade")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListTaskByPriorityReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListTaskByPriorityReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListTaskByPriorityReportResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Relatório de tarefas por prioridade")]
    public async Task<IActionResult> GetTaskByPriorityReport(
        [FromServices] IGetListTaskByPriorityReportUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>>.Ok(
        List<GetListTaskByPriorityReportResponse> report)
        => _viewModel = base.Ok(new GenericResponse<List<GetListTaskByPriorityReportResponse>>(true, report, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListTaskByPriorityReportResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListTaskByPriorityReportResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
