using CrossCutting.Const;
using Domain.Common.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.DataAccess.Seeds;

public static class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        if (builder == null)
            throw new ArgumentNullException(nameof(builder));

        // User Roles
        builder.Entity<UserRole>()
               .HasData(
                    new UserRole(SystemConst.UserRoleManagerIdDefault, SystemConst.Manager),
                    new UserRole(SystemConst.UserRoleTeamIdDefault, SystemConst.Team)
               );

        // Users
        builder.Entity<User>()
               .HasData(
                   new User(1, SystemConst.UserRoleManagerIdDefault, "manager", "manager@eclipse.app", "1", true),
                   new User(2, SystemConst.UserRoleTeamIdDefault, "developer", "developer@eclipse.app", "1", true),
                   new User(3, SystemConst.UserRoleTeamIdDefault, "analyst", "analyst@eclipse.app", "1", true),
                   new User(4, SystemConst.UserRoleTeamIdDefault, "tester", "tester@eclipse.app", "1", true),
                   new User(5, SystemConst.UserRoleTeamIdDefault, "designer", "designer@eclipse.app", "1", true)
               );

        var currentDate = DateTime.UtcNow;
        var random = new Random();

        // Projects
        builder.Entity<Project>()
            .HasData(
                Enumerable.Range(1, 10).Select(projectId =>
                {
                    var totalTasks = 20;
                    var delayedPercentage = random.Next(20, 60);
                    var completedPercentage = random.Next(30, 70);

                    var delayedTasks = (totalTasks * delayedPercentage) / 100;
                    var completedTasks = (totalTasks * completedPercentage) / 100;
                    var onTrackTasks = totalTasks - delayedTasks - completedTasks;

                    var projectStatus = delayedTasks > onTrackTasks
                        ? StatusEnum.Pendente
                        : completedTasks > onTrackTasks
                            ? StatusEnum.Concluida
                            : StatusEnum.EmAndamento;

                    return new Project(
                        id: projectId,
                        userId: projectId % 5 + 1,
                        title: $"Projeto {projectId}",
                        description: $"Descrição do Projeto {projectId}",
                        expectedStartDate: currentDate.AddDays(-random.Next(30, 90)),
                        expectedEndDate: currentDate.AddDays(random.Next(-30, 60)),
                        status: projectStatus,
                        priority: random.Next(1, 4) == 1 ? PriorityEnum.Alta :
                                  random.Next(1, 4) == 2 ? PriorityEnum.Media : PriorityEnum.Baixa
                    );
                }).ToArray()
            );

        // Tasks
        builder.Entity<Task>()
            .HasData(
                Enumerable.Range(1, 10).SelectMany(projectId =>
                {
                    return Enumerable.Range(1, 20).Select(taskId =>
                    {
                        var isDelayed = random.Next(0, 100) < 40;
                        var isCompleted = random.Next(0, 100) < 50;
                        var hasStarted = random.Next(0, 100) < 70;

                        var expectedStartDate = currentDate.AddDays(-random.Next(15, 120));
                        var expectedEndDate = expectedStartDate.AddDays(random.Next(5, 20));
                        var actualEndDate = isCompleted
                            ? expectedEndDate.AddDays(-random.Next(1, 5))
                            : (DateTime?)null;

                        var status = isCompleted
                            ? StatusEnum.Concluida
                            : isDelayed
                                ? StatusEnum.Pendente
                                : hasStarted
                                    ? StatusEnum.EmAndamento
                                    : StatusEnum.Pendente;

                        return new Task(
                            id: ((projectId - 1) * 20) + taskId,
                            projectId: projectId,
                            userId: random.Next(1, 5),
                            title: $"Tarefa {taskId} do Projeto {projectId}",
                            description: $"Descrição da tarefa {taskId} para o projeto {projectId}.",
                            expectedStartDate: expectedStartDate,
                            expectedEndDate: expectedEndDate,
                            status: status,
                            priority: taskId % 4 == 0 ? PriorityEnum.Baixa : (taskId % 3 == 0 ? PriorityEnum.Alta : PriorityEnum.Media)
                        );
                    });
                }).ToArray()
            );
    }
}