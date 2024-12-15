using Application.UseCases.V1.Project.GetListSearchProject.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Project.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace WebApi.UseCases.V1.Project;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projetos", Name = "Projetos")]
[ApiController]
public class GetListSearchProjectController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Project>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchProjectController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListProjectResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar projetos com paginação")]
    public IActionResult GetListSearchProject(
       [FromServices] IGetListSearchProjectUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Project>>.Ok(
        GenericPaginationResponse<Domain.Entities.Project> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListProjectResponse>(true, new GetListProjectResponse().GetListProject(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Project>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListProjectResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Project>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListProjectResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}