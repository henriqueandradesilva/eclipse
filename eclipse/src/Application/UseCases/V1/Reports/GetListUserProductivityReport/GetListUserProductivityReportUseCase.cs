﻿using Application.UseCases.V1.Reports.GetListUserProductivityReport.Interfaces;
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

namespace Application.UseCases.V1.Reports.GetListUserProductivityReport;

public class GetListUserProductivityReportUseCase : IGetListUserProductivityReportUseCase
{
    private IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListUserProductivityReportUseCase(
        ITaskRepository taskRepository,
        NotificationHelper notificationHelper)
    {
        _taskRepository = taskRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var query =
            _taskRepository.GetAllWithIncludes(t => t.User);

        var result = await query?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var report = result
            .GroupBy(t => t.User)
            .Select(g => new GetListUserProductivityReportResponse
            {
                Usuario = g.Key.Name,
                TotalTarefa = g.Count(),
                TotalTarefaConcluida = g.Count(t => t.Status == StatusEnum.Concluida),
                TotalTarefaEmAndamento = g.Count(t => t.Status == StatusEnum.EmAndamento),
                TotalTarefaPendente = g.Count(t => t.Status == StatusEnum.Pendente),
                TempoMedioConclusaoEmDias = g.Where(t => t.Status == StatusEnum.Concluida)
                                             .Average(t => (t.ExpectedEndDate - t.ExpectedStartDate).TotalDays)
            })
            .ToList();

        _outputPort.Ok(report);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>> outputPort)
        => _outputPort = outputPort;
}