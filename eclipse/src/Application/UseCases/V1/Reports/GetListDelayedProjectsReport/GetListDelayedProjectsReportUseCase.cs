using Application.UseCases.V1.Reports.GetListDelayedProjectsReport.Interfaces;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using Domain.Common.Enums;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Reports.GetListDelayedProjectsReport;

public class GetListDelayedProjectsReportUseCase : IGetListDelayedProjectsReportUseCase
{
    private IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;

    public GetListDelayedProjectsReportUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var tasks = await _taskRepository.GetAllWithIncludes(t => t.Project, t => t.Project.User, t => t.User);

        if (tasks == null || !tasks.Any())
        {
            _outputPort.NotFound();
            return;
        }

        var delayedProjects = tasks
            .Where(t => t.Project.ExpectedEndDate < DateTime.UtcNow && t.ExpectedEndDate < DateTime.UtcNow && t.Status != StatusEnum.Concluida)
            .GroupBy(t => t.Project)
            .Select(g => new GetListDelayedProjectReportResponse
            {
                TituloProjeto = g.Key.Title,
                Usuario = g.Key.User.Name,
                TotalTarefaAtraso = g.Count(),
                ExpectativaDataVencimento = g.Key.ExpectedEndDate,
                Prioridade = g.Key.Priority
            })
            .ToList();

        _outputPort.Ok(delayedProjects);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>> outputPort)
        => _outputPort = outputPort;
}