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
                new User
                (
                   3,
                   SystemConst.UserRoleTeamIdDefault,
                    "design",
                    "design@eclipse.app",
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
                ),
                new Project(
                    id: 4,
                    userId: 2,
                    title: "Existing Project 4",
                    description: "Description 4",
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
                ),

                //Project 4 - 20 Tasks
                new Domain.Entities.Task(
                    id: 5,
                    projectId: 4,
                    userId: 1,
                    title: "Task 1 for Project 4",
                    description: "Description for Task 1",
                    expectedStartDate: DateTime.UtcNow.AddDays(-10),
                    expectedEndDate: DateTime.UtcNow.AddDays(5),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 6,
                    projectId: 4,
                    userId: 1,
                    title: "Task 2 for Project 4",
                    description: "Description for Task 2",
                    expectedStartDate: DateTime.UtcNow.AddDays(-9),
                    expectedEndDate: DateTime.UtcNow.AddDays(6),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 7,
                    projectId: 4,
                    userId: 2,
                    title: "Task 3 for Project 4",
                    description: "Description for Task 3",
                    expectedStartDate: DateTime.UtcNow.AddDays(-8),
                    expectedEndDate: DateTime.UtcNow.AddDays(7),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 8,
                    projectId: 4,
                    userId: 3,
                    title: "Task 4 for Project 4",
                    description: "Description for Task 4",
                    expectedStartDate: DateTime.UtcNow.AddDays(-7),
                    expectedEndDate: DateTime.UtcNow.AddDays(8),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 9,
                    projectId: 4,
                    userId: 1,
                    title: "Task 5 for Project 4",
                    description: "Description for Task 5",
                    expectedStartDate: DateTime.UtcNow.AddDays(-6),
                    expectedEndDate: DateTime.UtcNow.AddDays(9),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 10,
                    projectId: 4,
                    userId: 2,
                    title: "Task 6 for Project 4",
                    description: "Description for Task 6",
                    expectedStartDate: DateTime.UtcNow.AddDays(-5),
                    expectedEndDate: DateTime.UtcNow.AddDays(10),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 11,
                    projectId: 4,
                    userId: 3,
                    title: "Task 7 for Project 4",
                    description: "Description for Task 7",
                    expectedStartDate: DateTime.UtcNow.AddDays(-4),
                    expectedEndDate: DateTime.UtcNow.AddDays(11),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 12,
                    projectId: 4,
                    userId: 1,
                    title: "Task 8 for Project 4",
                    description: "Description for Task 8",
                    expectedStartDate: DateTime.UtcNow.AddDays(-3),
                    expectedEndDate: DateTime.UtcNow.AddDays(12),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 13,
                    projectId: 4,
                    userId: 2,
                    title: "Task 9 for Project 4",
                    description: "Description for Task 9",
                    expectedStartDate: DateTime.UtcNow.AddDays(-2),
                    expectedEndDate: DateTime.UtcNow.AddDays(13),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 14,
                    projectId: 4,
                    userId: 3,
                    title: "Task 10 for Project 4",
                    description: "Description for Task 10",
                    expectedStartDate: DateTime.UtcNow.AddDays(-1),
                    expectedEndDate: DateTime.UtcNow.AddDays(14),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 15,
                    projectId: 4,
                    userId: 1,
                    title: "Task 11 for Project 4",
                    description: "Description for Task 11",
                    expectedStartDate: DateTime.UtcNow,
                    expectedEndDate: DateTime.UtcNow.AddDays(15),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 16,
                    projectId: 4,
                    userId: 2,
                    title: "Task 12 for Project 4",
                    description: "Description for Task 12",
                    expectedStartDate: DateTime.UtcNow.AddDays(1),
                    expectedEndDate: DateTime.UtcNow.AddDays(16),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 17,
                    projectId: 4,
                    userId: 3,
                    title: "Task 13 for Project 4",
                    description: "Description for Task 13",
                    expectedStartDate: DateTime.UtcNow.AddDays(2),
                    expectedEndDate: DateTime.UtcNow.AddDays(17),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 18,
                    projectId: 4,
                    userId: 1,
                    title: "Task 14 for Project 4",
                    description: "Description for Task 14",
                    expectedStartDate: DateTime.UtcNow.AddDays(3),
                    expectedEndDate: DateTime.UtcNow.AddDays(18),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 19,
                    projectId: 4,
                    userId: 2,
                    title: "Task 15 for Project 4",
                    description: "Description for Task 15",
                    expectedStartDate: DateTime.UtcNow.AddDays(4),
                    expectedEndDate: DateTime.UtcNow.AddDays(19),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 20,
                    projectId: 4,
                    userId: 3,
                    title: "Task 16 for Project 4",
                    description: "Description for Task 16",
                    expectedStartDate: DateTime.UtcNow.AddDays(5),
                    expectedEndDate: DateTime.UtcNow.AddDays(20),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 21,
                    projectId: 4,
                    userId: 1,
                    title: "Task 17 for Project 4",
                    description: "Description for Task 17",
                    expectedStartDate: DateTime.UtcNow.AddDays(6),
                    expectedEndDate: DateTime.UtcNow.AddDays(21),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                ),
                new Domain.Entities.Task(
                    id: 22,
                    projectId: 4,
                    userId: 2,
                    title: "Task 18 for Project 4",
                    description: "Description for Task 18",
                    expectedStartDate: DateTime.UtcNow.AddDays(7),
                    expectedEndDate: DateTime.UtcNow.AddDays(22),
                    status: StatusEnum.EmAndamento,
                    priority: PriorityEnum.Baixa
                ),
                new Domain.Entities.Task(
                    id: 23,
                    projectId: 4,
                    userId: 3,
                    title: "Task 19 for Project 4",
                    description: "Description for Task 19",
                    expectedStartDate: DateTime.UtcNow.AddDays(8),
                    expectedEndDate: DateTime.UtcNow.AddDays(23),
                    status: StatusEnum.Concluida,
                    priority: PriorityEnum.Alta
                ),
                new Domain.Entities.Task(
                    id: 24,
                    projectId: 4,
                    userId: 1,
                    title: "Task 20 for Project 4",
                    description: "Description for Task 20",
                    expectedStartDate: DateTime.UtcNow.AddDays(9),
                    expectedEndDate: DateTime.UtcNow.AddDays(24),
                    status: StatusEnum.Pendente,
                    priority: PriorityEnum.Media
                )
            };

            dbContext.Set<Domain.Entities.Task>().AddRange(tasks);
        }

        await dbContext.SaveChangesAsync();
    }
}