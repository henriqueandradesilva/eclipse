using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Interfaces;
using System.Collections.Generic;

namespace Application.UseCases.V1.Reports.GetListUserProductivityReport.Interfaces;

public interface IGetListUserProductivityReportUseCase
{
    System.Threading.Tasks.Task Execute(
        long userId);

    void SetOutputPort(
        IOutputPortWithNotFound<List<GetListUserProductivityReportResponse>> outputPort);
}