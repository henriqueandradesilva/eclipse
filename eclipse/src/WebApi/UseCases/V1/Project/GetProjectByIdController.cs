using Application.UseCases.V1.Project.GetProjectById.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Project;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projetos", Name = "Projetos")]
[ApiController]
public class GetProjectByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Project>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetProjectByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetProjectResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um projeto por id")]
    public async Task<IActionResult> GetProject(
        [FromServices] IGetProjectByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Project>.Ok(
        Domain.Entities.Project project)
        => _viewModel = base.Ok(new GenericResponse<GetProjectResponse>(true, new GetProjectResponse().GetProject(project), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Project>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetProjectResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Project>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetProjectResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}