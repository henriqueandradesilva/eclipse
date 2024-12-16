using Application.UseCases.V1.AuditLog.PostAuditLog;
using Application.UseCases.V1.Task.PutTask;
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


public class PutTaskUseCaseTests
{
    /// <summary>
    /// Alteração de tarefas
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Success_When_Task_Is_Update_Successfully()
    {
        // Arrange
        var task = new Domain.Entities.Task(
            id: 1,
            projectId: 1,
            userId: 1,
            title: "Update Task 1",
            description: "Description 1",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.EmAndamento,
            priority: Domain.Common.Enums.PriorityEnum.Media
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        #region Audit Log

        var mockAuditLogRepository = new Mock<IAuditLogRepository>();

        mockAuditLogRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.AuditLog, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.AuditLog, bool>> predicate) =>
                dbContext.Set<Domain.Entities.AuditLog>().Where(predicate));

        var mockPostAuditLogUseCase = new PostAuditLogUseCase(
            mockUnitOfWork.Object,
            mockAuditLogRepository.Object,
            mockNotificationHelper.Object
        );

        #endregion

        var useCase = new PutTaskUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(task);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockOutputPort.Verify(op => op.Ok(It.IsAny<Domain.Entities.Task>()), Times.Once);
    }

    [Fact]
    public async Task Execute_Should_Log_History_When_Task_Is_Updated()
    {
        // Arrange
        var task = new Domain.Entities.Task(
            id: 1,
            projectId: 1,
            userId: 1,
            title: "Updated Task Title",
            description: "Updated Description",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.EmAndamento,
            priority: Domain.Common.Enums.PriorityEnum.Media
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        #region Audit Log

        var mockAuditLogRepository = new Mock<IAuditLogRepository>();

        mockAuditLogRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.AuditLog, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.AuditLog, bool>> predicate) =>
                dbContext.Set<Domain.Entities.AuditLog>().Where(predicate));

        var mockPostAuditLogUseCase = new PostAuditLogUseCase(
            mockUnitOfWork.Object,
            mockAuditLogRepository.Object,
            mockNotificationHelper.Object
        );

        #endregion

        var useCase = new PutTaskUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(task);

        // Assert
        mockAuditLogRepository.Verify(repo => repo.Add(It.Is<Domain.Entities.AuditLog>(auditLog =>
            auditLog.EntityId == task.Id &&
            auditLog.UserId == task.UserId
        )), Times.Exactly(8));

        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        mockOutputPort.Verify(op => op.Ok(It.IsAny<Domain.Entities.Task>()), Times.Once);
    }

    /// <summary>
    /// Verifica se não é permitido alterar a prioridade de uma tarefa após ela ter sido criada.
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Error_When_Trying_To_Update_Priority_Of_Existing_Task()
    {
        // Arrange
        var task = new Domain.Entities.Task(
            id: 1,
            projectId: 1,
            userId: 1,
            title: "Update Task 1",
            description: "Description 1",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.EmAndamento,
            priority: Domain.Common.Enums.PriorityEnum.Baixa
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        #region Audit Log

        var mockAuditLogRepository = new Mock<IAuditLogRepository>();

        mockAuditLogRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.AuditLog, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.AuditLog, bool>> predicate) =>
                dbContext.Set<Domain.Entities.AuditLog>().Where(predicate));

        var mockPostAuditLogUseCase = new PostAuditLogUseCase(
            mockUnitOfWork.Object,
            mockAuditLogRepository.Object,
            mockNotificationHelper.Object
        );

        #endregion

        var useCase = new PutTaskUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(task);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.TaskNotChangedPriority), Times.Once);
        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }

    /// <summary>
    /// Verificar se tarefa já existe
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Error_When_Task_Already_Exists()
    {
        // Arrange
        var task = new Domain.Entities.Task(
            id: 2,
            projectId: 1,
            userId: 1,
            title: "Existing Task 1",
            description: "Description 1",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Media
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        #region Audit Log

        var mockAuditLogRepository = new Mock<IAuditLogRepository>();

        mockAuditLogRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.AuditLog, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.AuditLog, bool>> predicate) =>
                dbContext.Set<Domain.Entities.AuditLog>().Where(predicate));

        var mockPostAuditLogUseCase = new PostAuditLogUseCase(
            mockUnitOfWork.Object,
            mockAuditLogRepository.Object,
            mockNotificationHelper.Object
        );

        #endregion

        var useCase = new PutTaskUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
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
            id: 1,
            projectId: 1,
            userId: 1,
            title: "Update Task 1",
            description: "Description 1",
            expectedStartDate: DateTime.UtcNow.AddDays(5),
            expectedEndDate: DateTime.UtcNow,
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Media
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.Task>>();

        #region Audit Log

        var mockAuditLogRepository = new Mock<IAuditLogRepository>();

        mockAuditLogRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.AuditLog, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.AuditLog, bool>> predicate) =>
                dbContext.Set<Domain.Entities.AuditLog>().Where(predicate));

        var mockPostAuditLogUseCase = new PostAuditLogUseCase(
            mockUnitOfWork.Object,
            mockAuditLogRepository.Object,
            mockNotificationHelper.Object
        );

        #endregion

        var useCase = new PutTaskUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
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