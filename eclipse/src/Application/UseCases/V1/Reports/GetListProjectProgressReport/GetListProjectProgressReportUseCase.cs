using Application.UseCases.V1.Reports.GetListProjectProgressReport.Interfaces;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using Domain.Common.Enums;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Reports.GetListProjectProgressReport;

public class GetListProjectProgressReportUseCase : IGetListProjectProgressReportUseCase
{
    private IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;

    public GetListProjectProgressReportUseCase(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task Execute()
    {
        var listTask =
            await _taskRepository.GetAllWithIncludes(t => t.Project);

        if (listTask == null || !listTask.Any())
        {
            _outputPort.NotFound();

            return;
        }

        var report = listTask
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