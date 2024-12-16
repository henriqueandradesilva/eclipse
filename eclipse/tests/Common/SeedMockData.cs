using CrossCutting.Const;
using Domain.Common.Enums;
using Domain.Entities;
using Infrastructure.DataAccess;

namespace tests.Common;

public static class SeedMockData
{
    public static async System.Threading.Tasks.Task Init(
        EclipseDbContext dbContext,
        bool initUserRole,
        bool initUser,
        bool initProject,
        bool initTask)
    {

        if (initUserRole)
        {
            var userRoles = new List<UserRole>
            {
                new UserRole(id: 1, name: "Manager"),
                new UserRole(id: 2, name: "Team")
            };

            dbContext.Set<UserRole>().AddRange(userRoles);
        }

        if (initUser)
        {
            var users = new List<User>
            {
                new User
                (
                   1,
                   SystemConst.UserRoleManagerIdDefault,
                    "Manager",
                    "manager@eclipse.app",
                    "1",
                    true
                ),
                new User
                (
                   2,
                   SystemConst.UserRoleTeamIdDefault,
                    "Developer",
                    "developer@eclipse.app",
                    "1",
                    true
                ),
            };

            dbContext.Set<User>().AddRange(users);
        }

        if (initProject)
        {
            var projects = new List<Project>
            {
                new Project(
                    id: 1,
                    userId: 1,
                    title: "Existing Project 1",
                    description: "Description 1",
                    expectedStartDate: DateTime.UtcNow.AddDays(-10),
                    expectedEndDate: DateTime.UtcNow.AddDays(5),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Baixa
                ),
                new Project(
                    id: 2,
                    userId: 1,
                    title: "Existing Project 2",
                    description: "Description 2",
                    expectedStartDate: DateTime.UtcNow.AddDays(-20),
                    expectedEndDate: DateTime.UtcNow.AddDays(10),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Alta
                ),
                new Project(
                    id: 3,
                    userId: 2,
                    title: "Existing Project 3",
                    description: "Description 3",
                    expectedStartDate: DateTime.UtcNow.AddDays(-15),
                    expectedEndDate: DateTime.UtcNow.AddDays(15),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Media
                    )
            };

            dbContext.Set<Project>().AddRange(projects);
        }

        if (initTask)
        {
            var tasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task(
                    id: 1,
                    projectId: 1,
                    userId: 1,
                    title: "Existing Task 1",
                    description: "Description 1",
                    expectedStartDate: DateTime.UtcNow.AddDays(-8),
                    expectedEndDate: DateTime.UtcNow.AddDays(2),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 2,
                    projectId: 1,
                    userId: 1,
                    title: "Existing Task 2",
                    description: "Description 2",
                    expectedStartDate: DateTime.UtcNow.AddDays(-7),
                    expectedEndDate: DateTime.UtcNow.AddDays(3),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 3,
                    projectId: 2,
                    userId: 2,
                    title: "Existing Task 3",
                    description: "Description 3",
                    expectedStartDate: DateTime.UtcNow.AddDays(-5),
                    expectedEndDate: DateTime.UtcNow.AddDays(5),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 4,
                    projectId: 3,
                    userId: 3,
                    title: "Existing Task 4",
                    description: "Description 4",
                    expectedStartDate: DateTime.UtcNow.AddDays(-3),
                    expectedEndDate: DateTime.UtcNow.AddDays(10),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Media
                )
            };

            dbContext.Set<Domain.Entities.Task>().AddRange(tasks);
        }

        await dbContext.SaveChangesAsync();
    }
}