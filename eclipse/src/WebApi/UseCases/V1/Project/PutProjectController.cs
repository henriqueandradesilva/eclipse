using Application.UseCases.V1.Project.PutProject.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Project.Request;
using CrossCutting.Dtos.Project.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Project;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projetos", Name = "Projetos")]
[ApiController]
public class PutProjectController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Project>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutProjectController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutProjectResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um projeto por id")]
    public async Task<IActionResult> PutProject(
        [FromServices] IPutProjectUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutProjectRequest putProjectRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Project project =
            new Domain.Entities.Project(
            id,
            putProjectRequest.UsuarioId,
            putProjectRequest.Titulo,
            putProjectRequest.Descricao,
            putProjectRequest.DataInicio,
            putProjectRequest.DataVencimento,
            putProjectRequest.Status,
            putProjectRequest.Prioridade);

        await useCase.Execute(project);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Project>.Ok(
        Domain.Entities.Project project)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.ProjectUpdated);

        _viewModel = base.Ok(new GenericResponse<PutProjectResponse>(true, new PutProjectResponse(project), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Project>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutProjectResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}