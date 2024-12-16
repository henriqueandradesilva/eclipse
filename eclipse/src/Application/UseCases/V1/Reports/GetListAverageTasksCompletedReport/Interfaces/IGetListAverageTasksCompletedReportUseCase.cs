using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport.Interfaces;

public interface IGetListAverageTasksCompletedReportUseCase
{
    System.Threading.Tasks.Task Execute(
        long userId,
        int daysInterval);

    void SetOutputPort(
        IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>> outputPort);
}