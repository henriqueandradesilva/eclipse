using Application.UseCases.V1.Reports.GetListProjectProgressReport.Interfaces;
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

namespace Application.UseCases.V1.Reports.GetListProjectProgressReport;

public class GetListProjectProgressReportUseCase : IGetListProjectProgressReportUseCase
{
    private IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListProjectProgressReportUseCase(
        ITaskRepository taskRepository,
        NotificationHelper notificationHelper)
    {
        _taskRepository = taskRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var query =
             _taskRepository.GetAllWithIncludes(t => t.Project);

        var result = await query?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var report = result
            .GroupBy(t => t.Project)
            .Select(g => new GetListProjectProgressReportResponse
            {
                ProjetoTitulo = g.Key.Title,
                TotalTarefa = g.Count(),
                TotalTarefaConcluida = g.Count(t => t.Status == StatusEnum.Concluida),
                TotalTarefaPendente = g.Count(t => t.Status == StatusEnum.Pendente),
                TotalTarefaEmAndamento = g.Count(t => t.Status == StatusEnum.EmAndamento),
                Prioridade = g.Key.Priority
            })
            .ToList();

        _outputPort.Ok(report);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>> outputPort)
        => _outputPort = outputPort;
}