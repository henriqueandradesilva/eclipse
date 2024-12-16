using Application.UseCases.V1.Task.GetListSearchTask;
using Application.UseCases.V1.Task.PostTask;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
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

namespace eclipse.tests;

public class TaskUseCasesTests
{
    //Listagem de tarefas
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

    //Criação de tarefas
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Success_When_Task_Is_Created_Successfully()
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

    //Verificar se tarefa já existe
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Error_When_Task_Already_Exists()
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

    //Informar uma data de inicio maior que a data de vencimento
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Error_When_Invalid_Task_Data_Provided()
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