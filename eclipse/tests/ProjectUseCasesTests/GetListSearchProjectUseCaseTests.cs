using Application.UseCases.V1.Project.GetListSearchProject;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using tests.Common;

namespace tests.ProjectUseCasesTests;

public class GetListSearchProjectUseCaseTests
{
    /// <summary>
    /// Listagem de projetos
    /// </summary>
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Filtered_Projects_By_UserId_And_Pagination()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}")
            .Options;

        using var dbContext = new EclipseDbContext(options);

        await SeedMockData.Init(dbContext, true, true, true, false);

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
}