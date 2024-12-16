using Application.UseCases.V1.Task.DeleteTask;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.TaskUseCasesTests;

public class DeleteTaskUseCaseTests
{
    /// <summary>
    /// Remover tarefa
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Success_When_Task_Is_Deleted_Successfully()
    {
        // Arrange
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
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Domain.Entities.Task>>();

        var useCase = new DeleteTaskUseCase(
            mockUnitOfWork.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(1);

        // Assert
        mockTaskRepository.Verify(repo => repo.Delete(It.IsAny<Domain.Entities.Task>()), Times.Once);
        mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        mockOutputPort.Verify(op => op.Ok(It.IsAny<Domain.Entities.Task>()), Times.Once);
    }

    /// <summary>
    /// Tarefa não encontrada
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_NotFound_When_Task_Is_Not_Found()
    {
        // Arrange
        var taskId = 999L; // ID de tarefa inexistente

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
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Domain.Entities.Task>>();

        var useCase = new DeleteTaskUseCase(
            mockUnitOfWork.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(taskId);

        // Assert
        mockOutputPort.Verify(op => op.NotFound(), Times.Once);
    }
}
