using Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport.Interfaces;
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

namespace Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport;

public class GetListAverageTasksCompletedReportUseCase : IGetListAverageTasksCompletedReportUseCase
{
    private IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAverageTasksCompletedReportUseCase(
        ITaskRepository taskRepository,
        NotificationHelper notificationHelper)
    {
        _taskRepository = taskRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        int daysInterval)
    {
        var intervalStartDate = DateTime.UtcNow.AddDays(-daysInterval);

        var query =
            _taskRepository.GetAllWithIncludes(t => t.User);

        var result = await query?.Where(c => c.ExpectedEndDate >= intervalStartDate)?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var averageTasks = result
            .Where(t => t.Status == StatusEnum.Concluida && t.ExpectedEndDate >= intervalStartDate)
            .GroupBy(t => t.User)
            .Select(g => new GetListAverageTasksCompletedReportResponse
            {
                Usuario = g.Key.Name,
                TotalTarefaConcluida = g.Count(),
                MediaPorDia = g.Count() / (double)daysInterval
            })
        .ToList();

        if (!averageTasks.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, $"Nenhuma tarefa concluída nos últimos {daysInterval} dias.");

            _outputPort.NotFound();

            return;
        }

        _outputPort.Ok(averageTasks);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>> outputPort)
        => _outputPort = outputPort;
}