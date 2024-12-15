using Application.UseCases.V1.Reports.GetListDelayedProjectsReport.Interfaces;
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
public class DelayedProjectsReportController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DelayedProjectsReportController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("projetos-atrasados")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListDelayedProjectReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListDelayedProjectReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListDelayedProjectReportResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Relatório de projetos atrasados")]
    public async Task<IActionResult> GetDelayedProjectsReport(
        [FromServices] IGetListDelayedProjectsReportUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>>.Ok(
        List<GetListDelayedProjectReportResponse> report)
        => _viewModel = base.Ok(new GenericResponse<List<GetListDelayedProjectReportResponse>>(true, report, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListDelayedProjectReportResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListDelayedProjectReportResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
