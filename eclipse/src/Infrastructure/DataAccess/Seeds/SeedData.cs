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

        builder.Entity<UserRole>()
               .HasData(
                    new UserRole(SystemConst.UserRoleManagerIdDefault, SystemConst.Manager),
                    new UserRole(SystemConst.UserRoleDeveloperIdDefault, SystemConst.Developer));

        builder.Entity<User>()
               .HasData(new User(SystemConst.UserManagerIdDefault, 1, "manager", "manager@eclipse.app", "1", true),
                        new User(SystemConst.UserDeveloperIdDefault, 1, "developer", "developer@eclipse.app", "1", true));

        builder.Entity<Project>()
               .HasData(
           new Project(1, SystemConst.UserDeveloperIdDefault, "Plataforma de Gestão de Tarefas", "Desenvolvimento de uma plataforma para gestão e acompanhamento de tarefas.",
                       new DateTime(2024, 1, 15).ToUniversalTime(), new DateTime(2024, 6, 30).ToUniversalTime(), StatusEnum.EmAndamento, PriorityEnum.Alta),
           new Project(2, SystemConst.UserDeveloperIdDefault, "E-commerce para Produtos Orgânicos", "Implementação de um sistema de e-commerce para venda de produtos orgânicos.",
                       new DateTime(2024, 3, 1).ToUniversalTime(), new DateTime(2024, 8, 15).ToUniversalTime(), StatusEnum.Pendente, PriorityEnum.Media),
           new Project(3, SystemConst.UserDeveloperIdDefault, "Sistema de Controle de Estoque", "Criação de um sistema para controle e gerenciamento de estoque.",
                       new DateTime(2024, 2, 10).ToUniversalTime(), new DateTime(2024, 7, 1).ToUniversalTime(), StatusEnum.EmAndamento, PriorityEnum.Alta),
           new Project(4, SystemConst.UserDeveloperIdDefault, "Plataforma de Educação Online", "Plataforma para oferecer cursos online.",
                       new DateTime(2024, 4, 5).ToUniversalTime(), new DateTime(2024, 12, 1).ToUniversalTime(), StatusEnum.Pendente, PriorityEnum.Alta),
           new Project(5, SystemConst.UserDeveloperIdDefault, "Aplicativo de Gerenciamento Financeiro", "Aplicativo para gerenciar finanças pessoais.",
                       new DateTime(2024, 1, 20).ToUniversalTime(), new DateTime(2024, 6, 15).ToUniversalTime(), StatusEnum.EmAndamento, PriorityEnum.Alta),
           new Project(6, SystemConst.UserDeveloperIdDefault, "Sistema de Agendamento Médico", "Criação de um sistema para agendamentos e consultas médicas.",
                       new DateTime(2024, 3, 10).ToUniversalTime(), new DateTime(2024, 8, 30).ToUniversalTime(), StatusEnum.EmAndamento, PriorityEnum.Media),
           new Project(7, SystemConst.UserDeveloperIdDefault, "Marketplace de Serviços", "Desenvolvimento de um marketplace para serviços diversos.",
                       new DateTime(2024, 2, 5).ToUniversalTime(), new DateTime(2024, 9, 10).ToUniversalTime(), StatusEnum.Pendente, PriorityEnum.Alta),
           new Project(8, SystemConst.UserDeveloperIdDefault, "Sistema de Fidelidade", "Criação de um sistema para gerenciar programas de fidelidade.",
                       new DateTime(2024, 4, 1).ToUniversalTime(), new DateTime(2024, 7, 15).ToUniversalTime(), StatusEnum.EmAndamento, PriorityEnum.Media),
           new Project(9, SystemConst.UserDeveloperIdDefault, "Plataforma de Gerenciamento de Times", "Sistema para organizar tarefas e times em empresas.",
                       new DateTime(2024, 3, 15).ToUniversalTime(), new DateTime(2024, 10, 1).ToUniversalTime(), StatusEnum.Pendente, PriorityEnum.Alta),
           new Project(10, SystemConst.UserDeveloperIdDefault, "Aplicativo de Saúde e Bem-Estar", "Aplicativo para acompanhamento de hábitos saudáveis.",
                       new DateTime(2024, 1, 25).ToUniversalTime(), new DateTime(2024, 7, 20).ToUniversalTime(), StatusEnum.Pendente, PriorityEnum.Media)
       );

        builder.Entity<Task>()
               .HasData(
                   Enumerable.Range(1, 10).SelectMany(projectId =>
                       Enumerable.Range(1, 20).Select(taskId => new Task(
                           id: ((projectId - 1) * 20) + taskId,
                           projectId: projectId,
                           userId: taskId % 2 == 0 ? 2 : 1,
                           title: $"Tarefa {taskId} do Projeto {projectId}",
                           description: $"Descrição da tarefa {taskId} para o projeto {projectId}.",
                           expectedStartDate: new DateTime(2024, 1, 1).AddDays(taskId).ToUniversalTime(),
                           expectedEndDate: new DateTime(2024, 1, 1).AddDays(taskId + 5).ToUniversalTime(),
                           status: taskId % 2 == 0 ? StatusEnum.EmAndamento : StatusEnum.Pendente,
                           priority: taskId % 3 == 0 ? PriorityEnum.Alta : PriorityEnum.Media
                       ))
                   ).ToArray()
               );
    }
}
