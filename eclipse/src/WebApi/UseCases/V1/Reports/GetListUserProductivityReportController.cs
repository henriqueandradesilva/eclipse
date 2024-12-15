using Application.UseCases.V1.Reports.GetListUserProductivityReport.Interfaces;
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
public class GetListUserProductivityReportController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListUserProductivityReportController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("produtividade-usuario")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListUserProductivityReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListUserProductivityReportResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListUserProductivityReportResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Relatório de produtividade por usuário")]
    public async Task<IActionResult> GetUserProductivityReport(
        [FromServices] IGetListUserProductivityReportUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>>.Ok(
        List<GetListUserProductivityReportResponse> report)
        => _viewModel = base.Ok(new GenericResponse<List<GetListUserProductivityReportResponse>>(true, report, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListUserProductivityReportResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListUserProductivityReportResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}
