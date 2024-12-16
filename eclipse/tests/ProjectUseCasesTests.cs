using Application.UseCases.V1.Project.PostProject;
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

namespace eclipse.tests;

public class ProjectUseCasesTests
{
    [Fact]
    public async System.Threading.Tasks.Task Execute_Should_Return_Error_When_Project_Already_Exists()
    {
        // Arrange
        var project = new Project(
            id: 0, // Novo projeto
            userId: 1,
            title: "Existing Project", // T�tulo j� existente
            description: "Description",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Baixa
        );

        var existingProject = new Project(
            id: 1,
            userId: 1,
            title: "Existing Project", // Mesmo t�tulo
            description: "An existing project",
            expectedStartDate: DateTime.UtcNow,
            expectedEndDate: DateTime.UtcNow.AddDays(5),
            status: Domain.Common.Enums.StatusEnum.Pendente,
            priority: Domain.Common.Enums.PriorityEnum.Baixa
        );

        // Configurando o DbContext em mem�ria
        var options = new DbContextOptionsBuilder<EclipseDbContext>()
            .UseInMemoryDatabase("TestDatabase") // Nome da base de dados em mem�ria
            .Options;

        var dbContext = new EclipseDbContext(options);

        // Adicionando um projeto existente no banco de dados
        dbContext.Set<Project>().Add(existingProject);
        await dbContext.SaveChangesAsync();

        // Criando o mock do reposit�rio
        var mockProjectRepository = new Mock<IProjectRepository>();

        // Simulando o m�todo Where para retornar projetos do banco em mem�ria
        mockProjectRepository
            .Setup(repo => repo.Where(It.IsAny<Expression<Func<Project, bool>>>()))
            .Returns((Expression<Func<Project, bool>> predicate) =>
                dbContext.Set<Project>().Where(predicate)); // Retorna o IQueryable do banco em mem�ria

        // N�o precisamos mockar o FirstOrDefaultAsync, j� que vamos usar o DbContext real
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
        mockOutputPort.Verify(op => op.Error(), Times.Once); // M�todo Error foi chamado
    }
}