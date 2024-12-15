namespace CrossCutting.Dtos.Reports.Response;

public class GetListAverageTasksCompletedReportResponse
{
    public string Usuario { get; set; }

    public int TotalTarefaConcluida { get; set; }

    public double MediaPorDia { get; set; }
}
