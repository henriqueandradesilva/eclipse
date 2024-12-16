using Application.UseCases.V1.Project.GetListSearchProject;
using Application.UseCases.V1.Project.PostProject;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
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

namespace eclipse.tests;

public class ProjectUseCasesTests
{
    //Listagem de projetos
    [Fact]
    public void Execute_Should_Return_Filtered_Projects_By_UserId_And_Pagination()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        SeedMockData.Init(dbContext, true, true, true, false);

        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                dbContext.Set<Project>().Where(predicate));

        var mockNotificationHelper = new Mock<NotificationHelper>();

        var mockOutputPort = new Mock<IOutputPortWithNotFound<GenericPaginationResponse<Project>>>();

        var useCase = new GetListSearchProjectUseCase(
            mockProjectRepository.Object,
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
            op => op.Ok(It.Is<GenericPaginationResponse<Project>>(response =>
                response.ListaResultado.Count == 2 &&
                response.ListaResultado.Any(p => p.Title == "Existing Project 1") &&
                response.ListaResultado.Any(p => p.Title == "Existing Project 2") &&
                response.Total == 2)),
            Times.Once);

        mockOutputPort.Verify(op => op.NotFound(), Times.Never);
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    //Cria��o de projetos
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Success_When_Project_Is_Created_Successfully()
    {
        // Arrange
        var project = new Project(
            id: 0,
            userId: 1,
            title: "New Project",
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

        SeedMockData.Init(dbContext, true, true, false, false);

        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                dbContext.Set<Project>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Project>>();

        var useCase = new PostProjectUseCase(
            mockUnitOfWork.Object,
            mockProjectRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(project);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockOutputPort.Verify(op => op.Ok(project), Times.Once);
    }

    //Verificar se projeto j� existe
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Error_When_Project_Already_Exists()
    {
        // Arrange
        var project = new Project(
            id: 0,
            userId: 1,
            title: "Existing Project 1",
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

        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                dbContext.Set<Project>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Project>>();

        var useCase = new PostProjectUseCase(
            mockUnitOfWork.Object,
            mockProjectRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(project);

        // Assert
        mockNotificationHelper.Verify(nh => nh.Add(SystemConst.Error, MessageConst.ProjectExist), Times.Once);
        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }

    //Informar uma data de inicio maior que a data de vencimento
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Error_When_Invalid_Project_Data_Provided()
    {
        // Arrange
        var project = new Project(
            id: 0,
            userId: 1,
            title: "New Project",
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

        var mockProjectRepository = new Mock<IProjectRepository>();

        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                dbContext.Set<Project>().Where(predicate));

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockNotificationHelper = new Mock<NotificationHelper>();
        var mockOutputPort = new Mock<IOutputPort<Project>>();

        var useCase = new PostProjectUseCase(
            mockUnitOfWork.Object,
            mockProjectRepository.Object,
            mockNotificationHelper.Object
        );

        useCase.SetOutputPort(mockOutputPort.Object);

        // Act
        await useCase.Execute(project);

        // Assert
        mockNotificationHelper.Verify(
            nh => nh.Add(SystemConst.Error, MessageConst.MessageDatetimeError),
            Times.Once);

        mockOutputPort.Verify(op => op.Error(), Times.Once);
    }
}