using Application.UseCases.V1.AuditLog.PostAuditLog;
using Application.UseCases.V1.TaskComment.PostTaskComment;
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

namespace tests.TaskCommentCommentUseCasesTests;


public class PostTaskCommentCommentUseCaseTests
{
    /// <summary>
    /// Criação de comentário
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Success_When_TaskComment_Is_Created_Successfully()
    {
        // Arrange
        var taskComment = new Domain.Entities.TaskComment(
            id: 0,
            taskId: 1,
            userId: 2,
            description: "Description 2"
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskCommentRepository = new Mock<ITaskCommentRepository>();

        mockTaskCommentRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.TaskComment, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.TaskComment, bool>> predicate) =>
                dbContext.Set<Domain.Entities.TaskComment>().Where(predicate));

        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.User, bool>> predicate) =>
                dbContext.Set<Domain.Entities.User>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.TaskComment>>();

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

        var useCase = new PostTaskCommentUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
            mockTaskCommentRepository.Object,
            mockUserRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(taskComment);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockOutputPort.Verify(op => op.Ok(taskComment), Times.Once);
    }

    /// <summary>
    /// Verificar se comentário já existe
    /// </summary>
    [Fact]
    public async Task Execute_Should_Return_Error_When_TaskComment_Already_Exists()
    {
        // Arrange
        var taskComment = new Domain.Entities.TaskComment(
            id: 0,
            taskId: 1,
            userId: 1,
            description: "Description 1"
        );

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true, true);

        var mockTaskCommentRepository = new Mock<ITaskCommentRepository>();

        mockTaskCommentRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.TaskComment, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.TaskComment, bool>> predicate) =>
                dbContext.Set<Domain.Entities.TaskComment>().Where(predicate));

        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.User, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.User, bool>> predicate) =>
                dbContext.Set<Domain.Entities.User>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Domain.Entities.TaskComment>>();

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

        var useCase = new PostTaskCommentUseCase(
            mockUnitOfWork.Object,
            mockPostAuditLogUseCase,
            mockTaskCommentRepository.Object,
            mockUserRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(taskComment);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.TaskCommentExist), Times.Once);
        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }
}