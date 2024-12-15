using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Reports.GetListDelayedProjectsReport.Interfaces;

public interface IGetListDelayedProjectsReportUseCase
{
    System.Threading.Tasks.Task Execute();

    void SetOutputPort(
        IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>> outputPort);
}