using Domain.Common.Enums;
using System.Collections.Generic;

namespace CrossCutting.Dtos.Reports.Response;

public class GetListTaskByPriorityReportResponse
{
    public PriorityEnum Prioridade { get; set; }

    public int TotalTarefa { get; set; }

    public int TotalTarefaConcluida { get; set; }

    public int TotalTarefaPendente { get; set; }

    public int TotalTarefaEmAndamento { get; set; }

    public List<string> ListaTopUsuario { get; set; }
}