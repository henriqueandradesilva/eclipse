using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Reports.GetListTaskByPriorityReport.Interfaces;

public interface IGetListTaskByPriorityReportUseCase
{
    System.Threading.Tasks.Task Execute(
        long userId);

    void SetOutputPort(
        IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>> outputPort);
}