using Application.UseCases.V1.Reports.GetListTaskByPriorityReport.Interfaces;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using Domain.Common.Enums;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

public class GetListTaskByPriorityReportUseCase : IGetListTaskByPriorityReportUseCase
{
    private IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>> _outputPort;
    private readonly ITaskRepository _taskRepository;

    public GetListTaskByPriorityReportUseCase(ITaskRepository taskRepository)
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
            .GroupBy(t => t.Priority)
            .Select(g => new GetListTaskByPriorityReportResponse
            {
                Prioridade = g.Key,
                TotalTarefa = g.Count(),
                TotalTarefaConcluida = g.Count(t => t.Status == StatusEnum.Concluida),
                TotalTarefaPendente = g.Count(t => t.Status == StatusEnum.Pendente),
                TotalTarefaEmAndamento = g.Count(t => t.Status == StatusEnum.EmAndamento),
                ListaTopUsuario = g.GroupBy(t => t.User)
                                   .OrderByDescending(u => u.Count())
                                   .Take(3)
                                   .Select(u => u.Key.Name)
                                   .ToList()
            })
            .ToList();

        _outputPort.Ok(report);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>> outputPort)
        => _outputPort = outputPort;
}
