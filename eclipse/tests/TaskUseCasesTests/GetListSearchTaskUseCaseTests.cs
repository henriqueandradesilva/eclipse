using Application.UseCases.V1.Task.GetListSearchTask;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.TaskUseCasesTests;

public class GetListSearchTaskUseCaseTests
{
    /// <summary>
    /// Listagem de tarefas por UserId
    /// </summary>
    [Fact]
    public void Execute_Should_Return_Filtered_Tasks_By_UserId_And_Pagination()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockNotificationHelper = new Mock<NotificationHelper>();

        var mockOutputPort = new Mock<IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>>();

        var useCase = new GetListSearchTaskUseCase(
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var listaRelacionamento =
            new List<Tuple<string, long>>
            {
                new Tuple<string, long>(SystemConst.FieldUserId, 1)
            };

        var request = new GenericSearchPaginationRequest
        {
            ListaRelacionamento = listaRelacionamento,
            TamanhoPagina = 10,
            PaginaAtual = 1,
            CampoOrdenacao = "Title",
            DirecaoOrdenacao = "asc"
        };

        // Act
        useCase.Execute(request);

        // Assert
        mockOutputPort.Verify(
            op => op.Ok(It.Is<GenericPaginationResponse<Domain.Entities.Task>>(response =>
                response.ListaResultado.Count == 2 &&
                response.ListaResultado.Any(p => p.Title == "Existing Task 1") &&
                response.ListaResultado.Any(p => p.Title == "Existing Task 2") &&
                response.Total == 2)),
            Times.Once);

        mockOutputPort.Verify(op => op.NotFound(), Times.Never);
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Listagem de tarefas por UserId quando o UserId não for encontrado
    /// </summary>
    [Fact]
    public void Execute_Should_Return_NotFound_When_UserId_Is_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate).Where(task => task.UserId == 99999));

        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>>();

        var useCase = new GetListSearchTaskUseCase(
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var listaRelacionamento =
            new List<Tuple<string, long>>
            {
                new Tuple<string, long>(SystemConst.FieldUserId, 99999)
            };

        var request = new GenericSearchPaginationRequest
        {
            ListaRelacionamento = listaRelacionamento,
            TamanhoPagina = 10,
            PaginaAtual = 1,
            CampoOrdenacao = "Title",
            DirecaoOrdenacao = "asc"
        };

        // Act
        useCase.Execute(request);

        // Assert
        mockOutputPort.Verify(
            op => op.NotFound(),
            Times.Once);
    }

    /// <summary>
    /// Listagem de tarefas por ProjectId
    /// </summary>
    [Fact]
    public void Execute_Should_Return_Filtered_Tasks_By_ProjectId_And_Pagination()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate));

        var mockNotificationHelper = new Mock<NotificationHelper>();

        var mockOutputPort = new Mock<IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>>();

        var useCase = new GetListSearchTaskUseCase(
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var listaRelacionamento =
            new List<Tuple<string, long>>
            {
                new Tuple<string, long>(SystemConst.FieldProjectId, 1)
            };

        var request = new GenericSearchPaginationRequest
        {
            ListaRelacionamento = listaRelacionamento,
            TamanhoPagina = 10,
            PaginaAtual = 1,
            CampoOrdenacao = "Title",
            DirecaoOrdenacao = "asc"
        };

        // Act
        useCase.Execute(request);

        // Assert
        mockOutputPort.Verify(
            op => op.Ok(It.Is<GenericPaginationResponse<Domain.Entities.Task>>(response =>
                response.ListaResultado.Count == 2 &&
                response.ListaResultado.Any(p => p.Title == "Existing Task 1") &&
                response.ListaResultado.Any(p => p.Title == "Existing Task 2") &&
                response.Total == 2)),
            Times.Once);

        mockOutputPort.Verify(op => op.NotFound(), Times.Never);
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Listagem de tarefas por ProjectId quando o ProjectId não for encontrado
    /// </summary>
    [Fact]
    public void Execute_Should_Return_NotFound_When_ProjectId_Is_Not_Found()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, true);

        var mockTaskRepository = new Mock<ITaskRepository>();

        mockTaskRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>()))
            .Returns((Expression<Func<Domain.Entities.Task, bool>> predicate) =>
                dbContext.Set<Domain.Entities.Task>().Where(predicate).Where(task => task.ProjectId == 99999));

        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Task>>>();

        var useCase = new GetListSearchTaskUseCase(
            mockTaskRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        var listaRelacionamento =
            new List<Tuple<string, long>>
            {
                new Tuple<string, long>(SystemConst.FieldProjectId, 99999)
            };

        var request = new GenericSearchPaginationRequest
        {
            ListaRelacionamento = listaRelacionamento,
            TamanhoPagina = 10,
            PaginaAtual = 1,
            CampoOrdenacao = "Title",
            DirecaoOrdenacao = "asc"
        };

        // Act
        useCase.Execute(request);

        // Assert
        mockOutputPort.Verify(
            op => op.NotFound(),
            Times.Once);
    }
}