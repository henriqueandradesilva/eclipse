using Application.UseCases.V1.Project.PostProject.Interfaces;
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
public class PostProjectController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Project>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostProjectController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostProjectResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostProjectResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo projeto")]
    public async Task<IActionResult> PostProject(
        [FromServices] IPostProjectUseCase useCase,
        [FromBody][Required] PostProjectRequest postProjectRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Project project =
            new Domain.Entities.Project(
            0,
            postProjectRequest.UsuarioId,
            postProjectRequest.Titulo,
            postProjectRequest.Descricao,
            postProjectRequest.DataInicio,
            postProjectRequest.DataVencimento,
            postProjectRequest.Status,
            postProjectRequest.Prioridade);

        await useCase.Execute(project);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Project>.Ok(
        Domain.Entities.Project project)
    {
        var uri = $"/projetos/{project.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.ProjectCreated);

        var response =
            new GenericResponse<PostProjectResponse>(true, new PostProjectResponse(project), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Project>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostProjectResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}