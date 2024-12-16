using Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport.Interfaces;
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
public class AverageTasksCompletedsReportController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public AverageTasksCompletedsReportController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("media-tarefas-concluidas")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListAverageTasksCompletedReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListAverageTasksCompletedReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListAverageTasksCompletedReportResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Relatório de médio de tarefas concluídas por usuário nos últimos dias.")]
    public async Task<IActionResult> GetAverageTasksCompletedsReport(
        [FromServices] IGetListAverageTasksCompletedReportUseCase useCase,
        [FromQuery] long usuarioId,
        [FromQuery] int intervaloDias = 30)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(usuarioId, intervaloDias);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>>.Ok(
        List<GetListAverageTasksCompletedReportResponse> report)
        => _viewModel = base.Ok(new GenericResponse<List<GetListAverageTasksCompletedReportResponse>>(true, report, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListAverageTasksCompletedReportResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListAverageTasksCompletedReportResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
