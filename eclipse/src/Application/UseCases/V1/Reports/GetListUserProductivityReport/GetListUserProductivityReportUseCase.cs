﻿using Application.UseCases.V1.Reports.GetListUserProductivityReport.Interfaces;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using Domain.Common.Enums;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Reports.GetListUserProductivityReport;

public class GetListUserProductivityReportUseCase : IGetListUserProductivityReportUseCase
{
    private IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;

    public GetListUserProductivityReportUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var tasks = await _taskRepository.GetAllWithIncludes(t => t.User);

        if (tasks == null || !tasks.Any())
        {
            _outputPort.NotFound();

            return;
        }

        var report = tasks
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
