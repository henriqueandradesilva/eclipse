using Application.UseCases.V1.Task.PostTask;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.TaskUseCasesTests;


public class PostTaskUseCaseTests
{
    /// <summary>
    /// Criação de tarefas
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Success_When_Task_Is_Created_Successfully()
    {
        // Arrange
        var task = new Domain.Entities.Task(
            id: 0,
            projectId: 1,
            userId: 1,
            title: "New Task",
            description: "Description",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Baixa
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, false);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        var useCase = new PostTaskUseCase(
            mockUnitOfWork.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(task);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockOutputPort.Verify(op => op.Ok(task), Times.Once);
    }

    /// <summary>
    /// Verificar se tarefa já existe
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Error_When_Task_Already_Exists()
    {
        // Arrange
        var task = new Domain.Entities.Task(
            id: 0,
            projectId: 1,
            userId: 1,
            title: "Existing Task 1",
            description: "Description",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Baixa
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        var useCase = new PostTaskUseCase(
            mockUnitOfWork.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(task);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.TaskExist), Times.Once);
        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }

    /// <summary>
    /// Informar uma data de inicio maior que a data de vencimento
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Error_When_Invalid_Task_Data_Provided()
    {
        // Arrange
        var Task = new Domain.Entities.Task(
            id: 0,
            projectId: 1,
            userId: 1,
            title: "New Task",
            description: "Description",
            expectedStartDate: DateTime.UtcNow.AddDays(5),
            expectedEndDate: DateTime.UtcNow,
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Baixa
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        var useCase = new PostTaskUseCase(
            mockUnitOfWork.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(Task);

        // Assert
        mockNotificationHelper.Verify(
            nh => nh.Add(SystemConst.Error, MessageConst.MessageDatetimeError),
            Times.Once);

        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }
}