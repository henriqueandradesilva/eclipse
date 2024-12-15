using Domain.Common.Enums;

namespace CrossCutting.Dtos.Reports.Response;

public class GetListProjectProgressReportResponse
{
    public string ProjetoTitulo { get; set; }

    public int TotalTarefa { get; set; }

    public int TotalTarefaConcluida { get; set; }

    public int TotalTarefaPendente { get; set; }

    public int TotalTarefaEmAndamento { get; set; }

    public PriorityEnum Prioridade { get; set; }
}