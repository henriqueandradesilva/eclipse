using Application.UseCases.V1.Project.DeleteProject.Interfaces;
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
public class DeleteProjectController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Project>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteProjectController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteProjectResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um projeto por id")]
    public async Task<IActionResult> DeleteProject(
        [FromServices] IDeleteProjectUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Project>.Ok(
        Domain.Entities.Project project)
        => _viewModel = base.Ok(new GenericResponse<DeleteProjectResponse>(true, new DeleteProjectResponse(project), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Project>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteProjectResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Project>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteProjectResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}