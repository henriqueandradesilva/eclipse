﻿using Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport;
using CrossCutting.Const;
using CrossCutting.Dtos.Reports.Response;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.ReportsUseCasesTests;

public class GetListAverageTasksCompletedReportUseCaseTests
{
    /// <summary>
    /// Testa se o método Execute retorna o relatório corretamente quando o usuário possui o papel de gerente.
    /// O teste verifica se o fluxo de sucesso é seguido (sem erro) e garante que o método Error() do OutputPort e o método 
    /// Add() do NotificationHelper não sejam chamados. O caso de uso deve gerar o relatório com base nas tarefas completadas
    /// sem adicionar mensagens de erro.
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Report_When_UserIsManager()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.User, bool>> predicate) =>
                dbContext.Set<Domain.Entities.User>().Where(predicate));

        var mockNotificationHelper = new Mock<NotificationHelper>();

        var mockOutputPort = new Mock<IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>>>();

        var useCase = new GetListAverageTasksCompletedReportUseCase(
            mockTaskRepository.Object,
            mockUserRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var requestUserId = 1;
        var daysInterval = 30;

        // Act
        await useCase.Execute(requestUserId, daysInterval);

        // Assert
        mockOutputPort.Verify(
            op => op.Ok(It.Is<List<GetListAverageTasksCompletedReportResponse>>(response =>
                response.Count > 0 &&
                response.All(r => r.Usuario != null)
            )),
            Times.Once
        );

        mockOutputPort.Verify(op => op.Error(), Times.Never);
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Testa se o método Execute retorna um erro quando o usuário não possui o papel de gerente.
    /// O teste verifica se, ao ser passado um usuário com papel diferente de gerente, o método Error() do OutputPort 
    /// é chamado e o método Add() do NotificationHelper é invocado para adicionar a mensagem de erro.
    /// Nenhum relatório é gerado e o processo de erro é executado corretamente.
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Report_When_UserIsNotManager()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.User, bool>> predicate) =>
                dbContext.Set<Domain.Entities.User>().Where(predicate));

        var mockNotificationHelper = new Mock<NotificationHelper>();

        var mockOutputPort = new Mock<IOutputPortWithNotFound<List<GetListAverageTasksCompletedReportResponse>>>();

        var useCase = new GetListAverageTasksCompletedReportUseCase(
            mockTaskRepository.Object,
            mockUserRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var requestUserId = 2;
        var daysInterval = 30;

        // Act
        await useCase.Execute(requestUserId, daysInterval);

        // Assert
        mockOutputPort.Verify(op => op.Error(), Times.Once);
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.MessageManager), Times.Once);
    }
}