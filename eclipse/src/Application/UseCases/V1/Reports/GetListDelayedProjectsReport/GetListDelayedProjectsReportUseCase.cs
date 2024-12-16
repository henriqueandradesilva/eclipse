using Application.UseCases.V1.Reports.GetListDelayedProjectsReport.Interfaces;
using CrossCutting.Const;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.UseCases.V1.Reports.GetListDelayedProjectsReport;

public class GetListDelayedProjectsReportUseCase : IGetListDelayedProjectsReportUseCase
{
    private IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>> _outputPort;
    private readonly ITaskRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListDelayedProjectsReportUseCase(
        ITaskRepository repository,
        IUserRepository userRepository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        long userId)
    {
        var user =
            await _userRepository?.Where(c => c.Id == userId)
                                 ?.FirstOrDefaultAsync();

        if (user.UserRoleId != SystemConst.UserRoleManagerIdDefault)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.MessageManager);

            _outputPort.Error();

            return;
        }

        var query =
            _repository.GetAllWithIncludes(t => t.Project, t => t.Project.User, t => t.User);

        var result = await query?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var delayedProjects = result
            .Where(t => t.Project.ExpectedEndDate < DateTime.UtcNow && t.ExpectedEndDate < DateTime.UtcNow && t.Status != StatusEnum.Concluida)
            .GroupBy(t => t.Project)
            .Select(g => new GetListDelayedProjectReportResponse
            {
                TituloProjeto = g.Key.Title,
                Usuario = g.Key.User.Name,
                TotalTarefaAtraso = g.Count(),
                ExpectativaDataVencimento = g.Key.ExpectedEndDate,
                Prioridade = g.Key.Priority
            })
            .ToList();

        _outputPort.Ok(delayedProjects);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListDelayedProjectReportResponse>> outputPort)
        => _outputPort = outputPort;
}