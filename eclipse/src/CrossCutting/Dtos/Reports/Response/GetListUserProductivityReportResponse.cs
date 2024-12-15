namespace CrossCutting.Dtos.Reports.Response;

public class GetListUserProductivityReportResponse
{
    public string Usuario { get; set; }

    public int TotalTarefa { get; set; }

    public int TotalTarefaConcluida { get; set; }

    public int TotalTarefaEmAndamento { get; set; }

    public int TotalTarefaPendente { get; set; }

    public double TempoMedioConclusaoEmDias { get; set; }
}