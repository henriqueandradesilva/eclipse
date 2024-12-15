using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Reports.GetListProjectProgressReport.Interfaces;

public interface IGetListProjectProgressReportUseCase
{
    System.Threading.Tasks.Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<GetListProjectProgressReportResponse>> outputPort);
}
