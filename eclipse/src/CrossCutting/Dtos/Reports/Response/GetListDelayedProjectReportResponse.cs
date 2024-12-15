using Domain.Common.Enums;
using System;

namespace CrossCutting.Dtos.Reports.Response;

public class GetListDelayedProjectReportResponse
{
    public string TituloProjeto { get; set; }

    public string Usuario { get; set; }

    public int TotalTarefaAtraso { get; set; }

    public DateTime ExpectativaDataVencimento { get; set; }

    public PriorityEnum Prioridade { get; set; }
}