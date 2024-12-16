using Application.UseCases.V1.Reports.GetListProjectProgressReport.Interfaces;
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
public class GetListProjectProgressReportController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListProjectProgressReportController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("progresso-geral")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListProjectProgressReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListProjectProgressReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListProjectProgressReportResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Relatório de progresso geral de projetos")]
    public async Task<IActionResult> GetProjectProgressReport(
        [FromServices] IGetListProjectProgressReportUseCase useCase,
        [FromQuery] long usuarioId)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(usuarioId);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>>.Ok(
        List<GetListProjectProgressReportResponse> report)
        => _viewModel = base.Ok(new GenericResponse<List<GetListProjectProgressReportResponse>>(true, report, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListProjectProgressReportResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListProjectProgressReportResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
