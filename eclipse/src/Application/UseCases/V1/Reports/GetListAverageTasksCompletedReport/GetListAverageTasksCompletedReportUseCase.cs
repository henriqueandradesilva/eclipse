﻿using Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport.Interfaces;
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

namespace Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport;

public class GetListAverageTasksCompletedReportUseCase : IGetListAverageTasksCompletedReportUseCase
{
    private IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>> _outputPort;
    private readonly ITaskRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAverageTasksCompletedReportUseCase(
        ITaskRepository repository,
        IUserRepository userRepository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _notificationHelper = notificationHelper;
    }

    public async System.Threading.Tasks.Task Execute(
        long userId,
        int daysInterval)
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

        var intervalStartDate = DateTime.UtcNow.AddDays(-daysInterval);

        var tasks =
            await _repository?.Where(c => c.ExpectedEndDate >= intervalStartDate)
                             ?.Include(c => c.User).ThenInclude(c => c.UserRole)
                             ?.ToListAsync();

        if (tasks == null || !tasks.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        var allUsers =
            await _userRepository?.Where(c => c.IsActive)
                                 ?.ToListAsync();

        var usersWithTasks = tasks.Select(t => t.User).Distinct().ToList();

        var usersWithoutTasks = allUsers
            .Where(u => !usersWithTasks.Any(ut => ut.Id == u.Id))
            .Select(u => new GetListAverageTasksCompletedReportResponse
            {
                Usuario = u.Name,
                TotalTarefaConcluida = 0,
                MediaPorDia = 0.0
            })
            .ToList();

        var averageTasks = usersWithTasks
            .GroupJoin(tasks, user => user.Id, task => task.User.Id, (user, userTasks) => new
            {
                User = user,
                CompletedTasks = userTasks.Where(t => t.Status == StatusEnum.Concluida).Count()
            })
            .Select(g => new GetListAverageTasksCompletedReportResponse
            {
                Usuario = g.User.Name,
                TotalTarefaConcluida = g.CompletedTasks,
                MediaPorDia = g.CompletedTasks / (double)daysInterval
            })
            .ToList();

        var combinedResult = averageTasks.Concat(usersWithoutTasks).ToList();

        if (!combinedResult.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MessageEmpty);

            _outputPort.NotFound();

            return;
        }

        _outputPort.Ok(combinedResult);
    }

    public void SetOutputPort(IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>> outputPort)
        => _outputPort = outputPort;
}