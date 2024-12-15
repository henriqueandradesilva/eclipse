using Application.UseCases.V1.Reports.GetListDelayedProjectsReport.Interfaces;
using CrossCutting.Const;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Reports.GetListDelayedProjectsReport;

public class GetListDelayedProjectsReportUseCase : IGetListDelayedProjectsReportUseCase
{
    private IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListDelayedProjectsReportUseCase(
        ITaskRepository taskRepository,
        NotificationHelper notificationHelper)
    {
        _taskRepository = taskRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var query =
            _taskRepository.GetAllWithIncludes(t => t.Project, t => t.Project.User, t => t.User);

        var result = await query?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var delayedProjects = result
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