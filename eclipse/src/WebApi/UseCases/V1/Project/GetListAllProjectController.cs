using Application.UseCases.V1.Project.GetListAllProject.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Project;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projetos", Name = "Projetos")]
[ApiController]
public class GetListAllProjectController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Project>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllProjectController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListProjectResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListProjectResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListProjectResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os projetos")]
    public async Task<IActionResult> GetListProject(
        [FromServices] IGetListAllProjectUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Project>>.Ok(
        List<Domain.Entities.Project> listProject)
        => _viewModel = base.Ok(new GenericResponse<List<GetListProjectResponse>>(true, new GetListProjectResponse().GetListProject(listProject), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Project>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListProjectResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Project>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListProjectResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}