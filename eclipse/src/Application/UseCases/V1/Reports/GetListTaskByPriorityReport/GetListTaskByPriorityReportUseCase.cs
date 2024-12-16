using Application.UseCases.V1.Reports.GetListTaskByPriorityReport.Interfaces;
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

public class GetListTaskByPriorityReportUseCase : IGetListTaskByPriorityReportUseCase
{
    private IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>> _outputPort;
    private readonly ITaskRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListTaskByPriorityReportUseCase(
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
            _repository.GetAllWithIncludes(t => t.User);

        var result = await query?.ToListAsync();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var report = result
            .GroupBy(t => t.Priority)
            .Select(g => new GetListTaskByPriorityReportResponse
            {
                Prioridade = g.Key,
                TotalTarefa = g.Count(),
                TotalTarefaConcluida = g.Count(t => t.Status == StatusEnum.Concluida),
                TotalTarefaPendente = g.Count(t => t.Status == StatusEnum.Pendente),
                TotalTarefaEmAndamento = g.Count(t => t.Status == StatusEnum.EmAndamento),
                ListaTopUsuario = g.GroupBy(t => t.User)
                                   .OrderByDescending(u => u.Count())
                                   .Take(3)
                                   .Select(u => u.Key.Name)
                                   .ToList()
            })
            .ToList();

        _outputPort.Ok(report);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListTaskByPriorityReportResponse>> outputPort)
        => _outputPort = outputPort;
}
