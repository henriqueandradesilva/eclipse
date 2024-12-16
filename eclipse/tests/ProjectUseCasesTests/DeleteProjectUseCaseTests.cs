using Application.UseCases.V1.Project.DeleteProject;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.ProjectUseCasesTests;

public class DeleteProjectUseCaseTests
{
    /// <summary>
    /// Remover projeto
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Success_When_Project_Is_Deleted_Successfully()
    {
        // Arrange
        var projectIdSuccess = 3;

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var project = dbContext.Set<Project>().FirstOrDefault(p => p.Id == 1);
        Assert.NotNull(project);

        var tasksInDb = dbContext.Set<Domain.Entities.Task>().ToList();
        Assert.True(tasksInDb.Any());

        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Project, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Project, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Project>().Where(predicate));

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(repo => repo.Any(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .ReturnsAsync((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Any(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Domain.Entities.Project>>();

        var useCase = new DeleteProjectUseCase(
            mockUnitOfWork.Object,
            mockProjectRepository.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(projectIdSuccess);

        // Assert
        mockTaskRepository.Verify(repo => repo.Any(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()), Times.Once);
        mockProjectRepository.Verify(repo => repo.Delete(It.IsAny<Domain.Entities.Project>()), Times.Once);
        mockOutputPort.Verify(op => op.Ok(It.IsAny<Domain.Entities.Project>()), Times.Once); // Verifica se Ok foi chamado
    }

    /// <summary>
    /// Um projeto com tarefas pendentes resulta em um erro, solicitando que as tarefas sejam concluídas ou removidas antes da exclusão do projeto
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Error_When_Project_Has_Pending_Tasks()
    {
        // Arrange
        var projectIdTaskPending = 1;

        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Project, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Project, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Project>().Where(predicate));

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(repo => repo.Any(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .ReturnsAsync((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Any(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Domain.Entities.Project>>();

        var useCase = new DeleteProjectUseCase(
            mockUnitOfWork.Object,
            mockProjectRepository.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(projectIdTaskPending);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.ProjectTaskPending), Times.Once);
        mockOutputPort.Verify(op => op.Error(), Times.Once);
        mockProjectRepository.Verify(repo => repo.Delete(It.IsAny<Domain.Entities.Project>()), Times.Never);
    }

    /// <summary>
    /// Projeto não encontrado
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_NotFound_When_Project_Is_Not_Found()
    {
        // Arrange
        var projectIdNotFound = 999L;

        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, true);

        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Project, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Project, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Project>().Where(predicate));

        var mockTaskRepository = new Mock<ITaskRepository>();
        mockTaskRepository
            .Setup(repo => repo.Any(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .ReturnsAsync((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Any(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<Domain.Entities.Project>>();

        var useCase = new DeleteProjectUseCase(
            mockUnitOfWork.Object,
            mockProjectRepository.Object,
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(projectIdNotFound);

        // Assert
        mockOutputPort.Verify(op => op.NotFound(), Times.Once);
    }
}