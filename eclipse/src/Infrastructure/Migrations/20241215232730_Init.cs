﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserRoleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    TypeEntity = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Priority = table.Column<string>(type: "varchar(50)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Priority = table.Column<string>(type: "varchar(50)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Task_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskComment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComment_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskComment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "DateUpdated", "Description" },
                values: new object[,]
                {
                    { 1L, null, "GERENTE" },
                    { 2L, null, "TIME" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "DateUpdated", "Email", "IsActive", "Name", "Password", "UserRoleId" },
                values: new object[,]
                {
                    { 1L, null, "MANAGER@ECLIPSE.APP", true, "MANAGER", "$2a$11$8a5E.sBrxo782jNm8dvXuutGBpGMpZdvcjuhtPUqAWGMxu3kPARJW", 1L },
                    { 2L, null, "DEVELOPER@ECLIPSE.APP", true, "DEVELOPER", "$2a$11$WY7Vi.Fcv388SBTKkOBCB.MGq/2Mz2CjlkpRCNMePgbbvzTI8Z6CG", 2L },
                    { 3L, null, "ANALYST@ECLIPSE.APP", true, "ANALYST", "$2a$11$zJ2I.nH2qPOgYZkDi4AbZ.0uXWvuvTUn4L4cYND/vl2PvcWw7ZaCy", 2L },
                    { 4L, null, "TESTER@ECLIPSE.APP", true, "TESTER", "$2a$11$lJb/evHNXWniNCvDYMkCke0jHgN8uplbpgd7PgiFloQIwA1v7cYLK", 2L },
                    { 5L, null, "DESIGNER@ECLIPSE.APP", true, "DESIGNER", "$2a$11$M2EG.gL5rcX/79.kIDKFAObQ7NIR4LcoQCTkB5JaYTC0UWmuC2eX.", 2L }
                });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "Id", "DateUpdated", "Description", "ExpectedEndDate", "ExpectedStartDate", "Priority", "Status", "Title", "UserId" },
                values: new object[,]
                {
                    { 1L, null, "Descrição do Projeto 1", new DateTime(2025, 2, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", "Pendente", "Projeto 1", 2L },
                    { 2L, null, "Descrição do Projeto 2", new DateTime(2024, 12, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", "Pendente", "Projeto 2", 3L },
                    { 3L, null, "Descrição do Projeto 3", new DateTime(2024, 11, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", "Pendente", "Projeto 3", 4L },
                    { 4L, null, "Descrição do Projeto 4", new DateTime(2025, 1, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", "Pendente", "Projeto 4", 5L },
                    { 5L, null, "Descrição do Projeto 5", new DateTime(2025, 1, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", "Pendente", "Projeto 5", 1L },
                    { 6L, null, "Descrição do Projeto 6", new DateTime(2024, 12, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", "Pendente", "Projeto 6", 2L },
                    { 7L, null, "Descrição do Projeto 7", new DateTime(2024, 12, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", "Concluida", "Projeto 7", 3L },
                    { 8L, null, "Descrição do Projeto 8", new DateTime(2025, 2, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", "Pendente", "Projeto 8", 4L },
                    { 9L, null, "Descrição do Projeto 9", new DateTime(2024, 11, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", "Pendente", "Projeto 9", 5L },
                    { 10L, null, "Descrição do Projeto 10", new DateTime(2024, 12, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", "Pendente", "Projeto 10", 1L }
                });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "DateUpdated", "Description", "ExpectedEndDate", "ExpectedStartDate", "Priority", "ProjectId", "Status", "Title", "UserId" },
                values: new object[,]
                {
                    { 1L, null, "Descrição da tarefa 1 para o projeto 1.", new DateTime(2024, 9, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "EmAndamento", "Tarefa 1 do Projeto 1", 3L },
                    { 2L, null, "Descrição da tarefa 2 para o projeto 1.", new DateTime(2024, 11, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Pendente", "Tarefa 2 do Projeto 1", 3L },
                    { 3L, null, "Descrição da tarefa 3 para o projeto 1.", new DateTime(2024, 10, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 1L, "EmAndamento", "Tarefa 3 do Projeto 1", 1L },
                    { 4L, null, "Descrição da tarefa 4 para o projeto 1.", new DateTime(2024, 9, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 1L, "Concluida", "Tarefa 4 do Projeto 1", 1L },
                    { 5L, null, "Descrição da tarefa 5 para o projeto 1.", new DateTime(2024, 10, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Pendente", "Tarefa 5 do Projeto 1", 1L },
                    { 6L, null, "Descrição da tarefa 6 para o projeto 1.", new DateTime(2024, 9, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 1L, "EmAndamento", "Tarefa 6 do Projeto 1", 3L },
                    { 7L, null, "Descrição da tarefa 7 para o projeto 1.", new DateTime(2024, 12, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Concluida", "Tarefa 7 do Projeto 1", 4L },
                    { 8L, null, "Descrição da tarefa 8 para o projeto 1.", new DateTime(2024, 10, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 1L, "Pendente", "Tarefa 8 do Projeto 1", 2L },
                    { 9L, null, "Descrição da tarefa 9 para o projeto 1.", new DateTime(2024, 9, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 1L, "EmAndamento", "Tarefa 9 do Projeto 1", 1L },
                    { 10L, null, "Descrição da tarefa 10 para o projeto 1.", new DateTime(2024, 10, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Concluida", "Tarefa 10 do Projeto 1", 4L },
                    { 11L, null, "Descrição da tarefa 11 para o projeto 1.", new DateTime(2024, 9, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Concluida", "Tarefa 11 do Projeto 1", 3L },
                    { 12L, null, "Descrição da tarefa 12 para o projeto 1.", new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 1L, "EmAndamento", "Tarefa 12 do Projeto 1", 2L },
                    { 13L, null, "Descrição da tarefa 13 para o projeto 1.", new DateTime(2024, 9, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "EmAndamento", "Tarefa 13 do Projeto 1", 4L },
                    { 14L, null, "Descrição da tarefa 14 para o projeto 1.", new DateTime(2024, 12, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Pendente", "Tarefa 14 do Projeto 1", 3L },
                    { 15L, null, "Descrição da tarefa 15 para o projeto 1.", new DateTime(2024, 11, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 1L, "Concluida", "Tarefa 15 do Projeto 1", 2L },
                    { 16L, null, "Descrição da tarefa 16 para o projeto 1.", new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 1L, "Concluida", "Tarefa 16 do Projeto 1", 1L },
                    { 17L, null, "Descrição da tarefa 17 para o projeto 1.", new DateTime(2024, 10, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Concluida", "Tarefa 17 do Projeto 1", 1L },
                    { 18L, null, "Descrição da tarefa 18 para o projeto 1.", new DateTime(2024, 8, 31, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 1L, "Concluida", "Tarefa 18 do Projeto 1", 2L },
                    { 19L, null, "Descrição da tarefa 19 para o projeto 1.", new DateTime(2024, 10, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 1L, "Concluida", "Tarefa 19 do Projeto 1", 4L },
                    { 20L, null, "Descrição da tarefa 20 para o projeto 1.", new DateTime(2024, 9, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 1L, "Pendente", "Tarefa 20 do Projeto 1", 3L },
                    { 21L, null, "Descrição da tarefa 1 para o projeto 2.", new DateTime(2024, 10, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Pendente", "Tarefa 1 do Projeto 2", 2L },
                    { 22L, null, "Descrição da tarefa 2 para o projeto 2.", new DateTime(2024, 11, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 2 do Projeto 2", 3L },
                    { 23L, null, "Descrição da tarefa 3 para o projeto 2.", new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 31, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 2L, "Pendente", "Tarefa 3 do Projeto 2", 3L },
                    { 24L, null, "Descrição da tarefa 4 para o projeto 2.", new DateTime(2024, 9, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 2L, "Concluida", "Tarefa 4 do Projeto 2", 4L },
                    { 25L, null, "Descrição da tarefa 5 para o projeto 2.", new DateTime(2024, 11, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 5 do Projeto 2", 3L },
                    { 26L, null, "Descrição da tarefa 6 para o projeto 2.", new DateTime(2024, 11, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 2L, "Pendente", "Tarefa 6 do Projeto 2", 1L },
                    { 27L, null, "Descrição da tarefa 7 para o projeto 2.", new DateTime(2024, 10, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "EmAndamento", "Tarefa 7 do Projeto 2", 1L },
                    { 28L, null, "Descrição da tarefa 8 para o projeto 2.", new DateTime(2024, 11, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 31, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 2L, "Concluida", "Tarefa 8 do Projeto 2", 4L },
                    { 29L, null, "Descrição da tarefa 9 para o projeto 2.", new DateTime(2024, 11, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 2L, "Pendente", "Tarefa 9 do Projeto 2", 4L },
                    { 30L, null, "Descrição da tarefa 10 para o projeto 2.", new DateTime(2024, 9, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 10 do Projeto 2", 1L },
                    { 31L, null, "Descrição da tarefa 11 para o projeto 2.", new DateTime(2024, 11, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Pendente", "Tarefa 11 do Projeto 2", 4L },
                    { 32L, null, "Descrição da tarefa 12 para o projeto 2.", new DateTime(2024, 11, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 2L, "Pendente", "Tarefa 12 do Projeto 2", 3L },
                    { 33L, null, "Descrição da tarefa 13 para o projeto 2.", new DateTime(2024, 9, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 13 do Projeto 2", 2L },
                    { 34L, null, "Descrição da tarefa 14 para o projeto 2.", new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 14 do Projeto 2", 2L },
                    { 35L, null, "Descrição da tarefa 15 para o projeto 2.", new DateTime(2024, 11, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 2L, "Pendente", "Tarefa 15 do Projeto 2", 4L },
                    { 36L, null, "Descrição da tarefa 16 para o projeto 2.", new DateTime(2024, 9, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 2L, "Concluida", "Tarefa 16 do Projeto 2", 4L },
                    { 37L, null, "Descrição da tarefa 17 para o projeto 2.", new DateTime(2024, 9, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 17 do Projeto 2", 4L },
                    { 38L, null, "Descrição da tarefa 18 para o projeto 2.", new DateTime(2024, 12, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 2L, "Concluida", "Tarefa 18 do Projeto 2", 1L },
                    { 39L, null, "Descrição da tarefa 19 para o projeto 2.", new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 31, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 2L, "Concluida", "Tarefa 19 do Projeto 2", 4L },
                    { 40L, null, "Descrição da tarefa 20 para o projeto 2.", new DateTime(2024, 12, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 2L, "Concluida", "Tarefa 20 do Projeto 2", 3L },
                    { 41L, null, "Descrição da tarefa 1 para o projeto 3.", new DateTime(2024, 11, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Concluida", "Tarefa 1 do Projeto 3", 1L },
                    { 42L, null, "Descrição da tarefa 2 para o projeto 3.", new DateTime(2024, 11, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Pendente", "Tarefa 2 do Projeto 3", 3L },
                    { 43L, null, "Descrição da tarefa 3 para o projeto 3.", new DateTime(2024, 9, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 3L, "EmAndamento", "Tarefa 3 do Projeto 3", 2L },
                    { 44L, null, "Descrição da tarefa 4 para o projeto 3.", new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 3L, "Concluida", "Tarefa 4 do Projeto 3", 1L },
                    { 45L, null, "Descrição da tarefa 5 para o projeto 3.", new DateTime(2024, 9, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Concluida", "Tarefa 5 do Projeto 3", 2L },
                    { 46L, null, "Descrição da tarefa 6 para o projeto 3.", new DateTime(2024, 12, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 3L, "Pendente", "Tarefa 6 do Projeto 3", 4L },
                    { 47L, null, "Descrição da tarefa 7 para o projeto 3.", new DateTime(2024, 9, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Concluida", "Tarefa 7 do Projeto 3", 2L },
                    { 48L, null, "Descrição da tarefa 8 para o projeto 3.", new DateTime(2024, 10, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 3L, "Pendente", "Tarefa 8 do Projeto 3", 4L },
                    { 49L, null, "Descrição da tarefa 9 para o projeto 3.", new DateTime(2024, 10, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 3L, "Concluida", "Tarefa 9 do Projeto 3", 3L },
                    { 50L, null, "Descrição da tarefa 10 para o projeto 3.", new DateTime(2024, 9, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Concluida", "Tarefa 10 do Projeto 3", 2L },
                    { 51L, null, "Descrição da tarefa 11 para o projeto 3.", new DateTime(2024, 10, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "EmAndamento", "Tarefa 11 do Projeto 3", 3L },
                    { 52L, null, "Descrição da tarefa 12 para o projeto 3.", new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 3L, "Pendente", "Tarefa 12 do Projeto 3", 2L },
                    { 53L, null, "Descrição da tarefa 13 para o projeto 3.", new DateTime(2024, 9, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "EmAndamento", "Tarefa 13 do Projeto 3", 2L },
                    { 54L, null, "Descrição da tarefa 14 para o projeto 3.", new DateTime(2024, 10, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Pendente", "Tarefa 14 do Projeto 3", 4L },
                    { 55L, null, "Descrição da tarefa 15 para o projeto 3.", new DateTime(2024, 9, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 3L, "EmAndamento", "Tarefa 15 do Projeto 3", 3L },
                    { 56L, null, "Descrição da tarefa 16 para o projeto 3.", new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 3L, "Concluida", "Tarefa 16 do Projeto 3", 1L },
                    { 57L, null, "Descrição da tarefa 17 para o projeto 3.", new DateTime(2024, 10, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Concluida", "Tarefa 17 do Projeto 3", 3L },
                    { 58L, null, "Descrição da tarefa 18 para o projeto 3.", new DateTime(2024, 10, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 3L, "Concluida", "Tarefa 18 do Projeto 3", 4L },
                    { 59L, null, "Descrição da tarefa 19 para o projeto 3.", new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 3L, "Concluida", "Tarefa 19 do Projeto 3", 3L },
                    { 60L, null, "Descrição da tarefa 20 para o projeto 3.", new DateTime(2024, 11, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 3L, "Concluida", "Tarefa 20 do Projeto 3", 4L },
                    { 61L, null, "Descrição da tarefa 1 para o projeto 4.", new DateTime(2024, 10, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Concluida", "Tarefa 1 do Projeto 4", 4L },
                    { 62L, null, "Descrição da tarefa 2 para o projeto 4.", new DateTime(2024, 11, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Concluida", "Tarefa 2 do Projeto 4", 2L },
                    { 63L, null, "Descrição da tarefa 3 para o projeto 4.", new DateTime(2024, 12, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 4L, "Concluida", "Tarefa 3 do Projeto 4", 1L },
                    { 64L, null, "Descrição da tarefa 4 para o projeto 4.", new DateTime(2024, 11, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 4L, "Concluida", "Tarefa 4 do Projeto 4", 1L },
                    { 65L, null, "Descrição da tarefa 5 para o projeto 4.", new DateTime(2024, 11, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "EmAndamento", "Tarefa 5 do Projeto 4", 4L },
                    { 66L, null, "Descrição da tarefa 6 para o projeto 4.", new DateTime(2024, 10, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 4L, "Pendente", "Tarefa 6 do Projeto 4", 3L },
                    { 67L, null, "Descrição da tarefa 7 para o projeto 4.", new DateTime(2024, 9, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 31, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Pendente", "Tarefa 7 do Projeto 4", 1L },
                    { 68L, null, "Descrição da tarefa 8 para o projeto 4.", new DateTime(2024, 11, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 4L, "Pendente", "Tarefa 8 do Projeto 4", 3L },
                    { 69L, null, "Descrição da tarefa 9 para o projeto 4.", new DateTime(2024, 11, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 4L, "Concluida", "Tarefa 9 do Projeto 4", 1L },
                    { 70L, null, "Descrição da tarefa 10 para o projeto 4.", new DateTime(2024, 10, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Pendente", "Tarefa 10 do Projeto 4", 2L },
                    { 71L, null, "Descrição da tarefa 11 para o projeto 4.", new DateTime(2024, 10, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Concluida", "Tarefa 11 do Projeto 4", 2L },
                    { 72L, null, "Descrição da tarefa 12 para o projeto 4.", new DateTime(2024, 9, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 4L, "Concluida", "Tarefa 12 do Projeto 4", 4L },
                    { 73L, null, "Descrição da tarefa 13 para o projeto 4.", new DateTime(2024, 11, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Pendente", "Tarefa 13 do Projeto 4", 4L },
                    { 74L, null, "Descrição da tarefa 14 para o projeto 4.", new DateTime(2024, 10, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Concluida", "Tarefa 14 do Projeto 4", 1L },
                    { 75L, null, "Descrição da tarefa 15 para o projeto 4.", new DateTime(2024, 9, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 4L, "Pendente", "Tarefa 15 do Projeto 4", 4L },
                    { 76L, null, "Descrição da tarefa 16 para o projeto 4.", new DateTime(2024, 11, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 4L, "Concluida", "Tarefa 16 do Projeto 4", 4L },
                    { 77L, null, "Descrição da tarefa 17 para o projeto 4.", new DateTime(2024, 12, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Concluida", "Tarefa 17 do Projeto 4", 4L },
                    { 78L, null, "Descrição da tarefa 18 para o projeto 4.", new DateTime(2024, 9, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 4L, "EmAndamento", "Tarefa 18 do Projeto 4", 2L },
                    { 79L, null, "Descrição da tarefa 19 para o projeto 4.", new DateTime(2024, 11, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 4L, "Concluida", "Tarefa 19 do Projeto 4", 4L },
                    { 80L, null, "Descrição da tarefa 20 para o projeto 4.", new DateTime(2024, 11, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 4L, "Pendente", "Tarefa 20 do Projeto 4", 4L },
                    { 81L, null, "Descrição da tarefa 1 para o projeto 5.", new DateTime(2024, 9, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Pendente", "Tarefa 1 do Projeto 5", 4L },
                    { 82L, null, "Descrição da tarefa 2 para o projeto 5.", new DateTime(2024, 9, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 2 do Projeto 5", 3L },
                    { 83L, null, "Descrição da tarefa 3 para o projeto 5.", new DateTime(2024, 12, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 5L, "Pendente", "Tarefa 3 do Projeto 5", 2L },
                    { 84L, null, "Descrição da tarefa 4 para o projeto 5.", new DateTime(2024, 10, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 5L, "Pendente", "Tarefa 4 do Projeto 5", 4L },
                    { 85L, null, "Descrição da tarefa 5 para o projeto 5.", new DateTime(2024, 9, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 5 do Projeto 5", 2L },
                    { 86L, null, "Descrição da tarefa 6 para o projeto 5.", new DateTime(2024, 11, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 31, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 5L, "Concluida", "Tarefa 6 do Projeto 5", 1L },
                    { 87L, null, "Descrição da tarefa 7 para o projeto 5.", new DateTime(2024, 12, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 7 do Projeto 5", 3L },
                    { 88L, null, "Descrição da tarefa 8 para o projeto 5.", new DateTime(2024, 10, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 5L, "Pendente", "Tarefa 8 do Projeto 5", 4L },
                    { 89L, null, "Descrição da tarefa 9 para o projeto 5.", new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 5L, "Pendente", "Tarefa 9 do Projeto 5", 1L },
                    { 90L, null, "Descrição da tarefa 10 para o projeto 5.", new DateTime(2024, 11, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 10 do Projeto 5", 4L },
                    { 91L, null, "Descrição da tarefa 11 para o projeto 5.", new DateTime(2024, 12, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 11 do Projeto 5", 3L },
                    { 92L, null, "Descrição da tarefa 12 para o projeto 5.", new DateTime(2024, 10, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 5L, "Pendente", "Tarefa 12 do Projeto 5", 3L },
                    { 93L, null, "Descrição da tarefa 13 para o projeto 5.", new DateTime(2024, 11, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Pendente", "Tarefa 13 do Projeto 5", 3L },
                    { 94L, null, "Descrição da tarefa 14 para o projeto 5.", new DateTime(2024, 11, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 14 do Projeto 5", 3L },
                    { 95L, null, "Descrição da tarefa 15 para o projeto 5.", new DateTime(2024, 9, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 5L, "Concluida", "Tarefa 15 do Projeto 5", 1L },
                    { 96L, null, "Descrição da tarefa 16 para o projeto 5.", new DateTime(2024, 12, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 5L, "Pendente", "Tarefa 16 do Projeto 5", 3L },
                    { 97L, null, "Descrição da tarefa 17 para o projeto 5.", new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "Concluida", "Tarefa 17 do Projeto 5", 1L },
                    { 98L, null, "Descrição da tarefa 18 para o projeto 5.", new DateTime(2024, 9, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 5L, "Concluida", "Tarefa 18 do Projeto 5", 2L },
                    { 99L, null, "Descrição da tarefa 19 para o projeto 5.", new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 5L, "EmAndamento", "Tarefa 19 do Projeto 5", 3L },
                    { 100L, null, "Descrição da tarefa 20 para o projeto 5.", new DateTime(2024, 12, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 5L, "Concluida", "Tarefa 20 do Projeto 5", 1L },
                    { 101L, null, "Descrição da tarefa 1 para o projeto 6.", new DateTime(2024, 11, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Concluida", "Tarefa 1 do Projeto 6", 3L },
                    { 102L, null, "Descrição da tarefa 2 para o projeto 6.", new DateTime(2024, 10, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Concluida", "Tarefa 2 do Projeto 6", 3L },
                    { 103L, null, "Descrição da tarefa 3 para o projeto 6.", new DateTime(2024, 10, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 6L, "Concluida", "Tarefa 3 do Projeto 6", 4L },
                    { 104L, null, "Descrição da tarefa 4 para o projeto 6.", new DateTime(2024, 9, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 6L, "Pendente", "Tarefa 4 do Projeto 6", 4L },
                    { 105L, null, "Descrição da tarefa 5 para o projeto 6.", new DateTime(2024, 11, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "EmAndamento", "Tarefa 5 do Projeto 6", 1L },
                    { 106L, null, "Descrição da tarefa 6 para o projeto 6.", new DateTime(2024, 10, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 6L, "Pendente", "Tarefa 6 do Projeto 6", 2L },
                    { 107L, null, "Descrição da tarefa 7 para o projeto 6.", new DateTime(2024, 10, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Concluida", "Tarefa 7 do Projeto 6", 1L },
                    { 108L, null, "Descrição da tarefa 8 para o projeto 6.", new DateTime(2024, 10, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 6L, "Concluida", "Tarefa 8 do Projeto 6", 3L },
                    { 109L, null, "Descrição da tarefa 9 para o projeto 6.", new DateTime(2024, 11, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 6L, "EmAndamento", "Tarefa 9 do Projeto 6", 1L },
                    { 110L, null, "Descrição da tarefa 10 para o projeto 6.", new DateTime(2024, 8, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Pendente", "Tarefa 10 do Projeto 6", 3L },
                    { 111L, null, "Descrição da tarefa 11 para o projeto 6.", new DateTime(2024, 10, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "EmAndamento", "Tarefa 11 do Projeto 6", 1L },
                    { 112L, null, "Descrição da tarefa 12 para o projeto 6.", new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 6L, "Pendente", "Tarefa 12 do Projeto 6", 2L },
                    { 113L, null, "Descrição da tarefa 13 para o projeto 6.", new DateTime(2024, 12, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Concluida", "Tarefa 13 do Projeto 6", 4L },
                    { 114L, null, "Descrição da tarefa 14 para o projeto 6.", new DateTime(2024, 10, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Concluida", "Tarefa 14 do Projeto 6", 4L },
                    { 115L, null, "Descrição da tarefa 15 para o projeto 6.", new DateTime(2024, 9, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 6L, "Concluida", "Tarefa 15 do Projeto 6", 1L },
                    { 116L, null, "Descrição da tarefa 16 para o projeto 6.", new DateTime(2024, 12, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 6L, "Pendente", "Tarefa 16 do Projeto 6", 4L },
                    { 117L, null, "Descrição da tarefa 17 para o projeto 6.", new DateTime(2024, 11, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Pendente", "Tarefa 17 do Projeto 6", 1L },
                    { 118L, null, "Descrição da tarefa 18 para o projeto 6.", new DateTime(2024, 10, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 6L, "Concluida", "Tarefa 18 do Projeto 6", 3L },
                    { 119L, null, "Descrição da tarefa 19 para o projeto 6.", new DateTime(2024, 10, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 6L, "Pendente", "Tarefa 19 do Projeto 6", 4L },
                    { 120L, null, "Descrição da tarefa 20 para o projeto 6.", new DateTime(2024, 10, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 6L, "Concluida", "Tarefa 20 do Projeto 6", 1L },
                    { 121L, null, "Descrição da tarefa 1 para o projeto 7.", new DateTime(2024, 11, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "EmAndamento", "Tarefa 1 do Projeto 7", 2L },
                    { 122L, null, "Descrição da tarefa 2 para o projeto 7.", new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Concluida", "Tarefa 2 do Projeto 7", 4L },
                    { 123L, null, "Descrição da tarefa 3 para o projeto 7.", new DateTime(2024, 11, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 7L, "Concluida", "Tarefa 3 do Projeto 7", 3L },
                    { 124L, null, "Descrição da tarefa 4 para o projeto 7.", new DateTime(2024, 11, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 7L, "Pendente", "Tarefa 4 do Projeto 7", 1L },
                    { 125L, null, "Descrição da tarefa 5 para o projeto 7.", new DateTime(2024, 12, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Pendente", "Tarefa 5 do Projeto 7", 4L },
                    { 126L, null, "Descrição da tarefa 6 para o projeto 7.", new DateTime(2024, 10, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 7L, "EmAndamento", "Tarefa 6 do Projeto 7", 1L },
                    { 127L, null, "Descrição da tarefa 7 para o projeto 7.", new DateTime(2024, 9, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "EmAndamento", "Tarefa 7 do Projeto 7", 3L },
                    { 128L, null, "Descrição da tarefa 8 para o projeto 7.", new DateTime(2024, 9, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 7L, "Concluida", "Tarefa 8 do Projeto 7", 2L },
                    { 129L, null, "Descrição da tarefa 9 para o projeto 7.", new DateTime(2024, 11, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 7L, "Concluida", "Tarefa 9 do Projeto 7", 4L },
                    { 130L, null, "Descrição da tarefa 10 para o projeto 7.", new DateTime(2024, 9, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "EmAndamento", "Tarefa 10 do Projeto 7", 2L },
                    { 131L, null, "Descrição da tarefa 11 para o projeto 7.", new DateTime(2024, 11, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Pendente", "Tarefa 11 do Projeto 7", 2L },
                    { 132L, null, "Descrição da tarefa 12 para o projeto 7.", new DateTime(2024, 11, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 7L, "Concluida", "Tarefa 12 do Projeto 7", 4L },
                    { 133L, null, "Descrição da tarefa 13 para o projeto 7.", new DateTime(2024, 11, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Concluida", "Tarefa 13 do Projeto 7", 2L },
                    { 134L, null, "Descrição da tarefa 14 para o projeto 7.", new DateTime(2024, 8, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Concluida", "Tarefa 14 do Projeto 7", 1L },
                    { 135L, null, "Descrição da tarefa 15 para o projeto 7.", new DateTime(2024, 12, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 7L, "Concluida", "Tarefa 15 do Projeto 7", 2L },
                    { 136L, null, "Descrição da tarefa 16 para o projeto 7.", new DateTime(2024, 11, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 7L, "Pendente", "Tarefa 16 do Projeto 7", 3L },
                    { 137L, null, "Descrição da tarefa 17 para o projeto 7.", new DateTime(2024, 10, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Concluida", "Tarefa 17 do Projeto 7", 2L },
                    { 138L, null, "Descrição da tarefa 18 para o projeto 7.", new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 7L, "Concluida", "Tarefa 18 do Projeto 7", 2L },
                    { 139L, null, "Descrição da tarefa 19 para o projeto 7.", new DateTime(2024, 9, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 7L, "Concluida", "Tarefa 19 do Projeto 7", 1L },
                    { 140L, null, "Descrição da tarefa 20 para o projeto 7.", new DateTime(2024, 9, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 7L, "Concluida", "Tarefa 20 do Projeto 7", 2L },
                    { 141L, null, "Descrição da tarefa 1 para o projeto 8.", new DateTime(2024, 10, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "EmAndamento", "Tarefa 1 do Projeto 8", 3L },
                    { 142L, null, "Descrição da tarefa 2 para o projeto 8.", new DateTime(2024, 11, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "EmAndamento", "Tarefa 2 do Projeto 8", 2L },
                    { 143L, null, "Descrição da tarefa 3 para o projeto 8.", new DateTime(2024, 9, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 8L, "EmAndamento", "Tarefa 3 do Projeto 8", 3L },
                    { 144L, null, "Descrição da tarefa 4 para o projeto 8.", new DateTime(2024, 9, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 8L, "Concluida", "Tarefa 4 do Projeto 8", 1L },
                    { 145L, null, "Descrição da tarefa 5 para o projeto 8.", new DateTime(2024, 11, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Pendente", "Tarefa 5 do Projeto 8", 3L },
                    { 146L, null, "Descrição da tarefa 6 para o projeto 8.", new DateTime(2024, 12, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 8L, "Concluida", "Tarefa 6 do Projeto 8", 4L },
                    { 147L, null, "Descrição da tarefa 7 para o projeto 8.", new DateTime(2024, 9, 12, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "EmAndamento", "Tarefa 7 do Projeto 8", 2L },
                    { 148L, null, "Descrição da tarefa 8 para o projeto 8.", new DateTime(2024, 9, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 8L, "EmAndamento", "Tarefa 8 do Projeto 8", 1L },
                    { 149L, null, "Descrição da tarefa 9 para o projeto 8.", new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 8L, "Pendente", "Tarefa 9 do Projeto 8", 4L },
                    { 150L, null, "Descrição da tarefa 10 para o projeto 8.", new DateTime(2024, 9, 25, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Concluida", "Tarefa 10 do Projeto 8", 4L },
                    { 151L, null, "Descrição da tarefa 11 para o projeto 8.", new DateTime(2024, 9, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Concluida", "Tarefa 11 do Projeto 8", 1L },
                    { 152L, null, "Descrição da tarefa 12 para o projeto 8.", new DateTime(2024, 9, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 8L, "EmAndamento", "Tarefa 12 do Projeto 8", 3L },
                    { 153L, null, "Descrição da tarefa 13 para o projeto 8.", new DateTime(2024, 10, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Concluida", "Tarefa 13 do Projeto 8", 4L },
                    { 154L, null, "Descrição da tarefa 14 para o projeto 8.", new DateTime(2024, 10, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Concluida", "Tarefa 14 do Projeto 8", 3L },
                    { 155L, null, "Descrição da tarefa 15 para o projeto 8.", new DateTime(2024, 9, 7, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 8L, "EmAndamento", "Tarefa 15 do Projeto 8", 3L },
                    { 156L, null, "Descrição da tarefa 16 para o projeto 8.", new DateTime(2024, 10, 24, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 8L, "Concluida", "Tarefa 16 do Projeto 8", 4L },
                    { 157L, null, "Descrição da tarefa 17 para o projeto 8.", new DateTime(2024, 9, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Pendente", "Tarefa 17 do Projeto 8", 3L },
                    { 158L, null, "Descrição da tarefa 18 para o projeto 8.", new DateTime(2024, 10, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 8L, "EmAndamento", "Tarefa 18 do Projeto 8", 3L },
                    { 159L, null, "Descrição da tarefa 19 para o projeto 8.", new DateTime(2024, 11, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 8L, "Pendente", "Tarefa 19 do Projeto 8", 1L },
                    { 160L, null, "Descrição da tarefa 20 para o projeto 8.", new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 8L, "Concluida", "Tarefa 20 do Projeto 8", 4L },
                    { 161L, null, "Descrição da tarefa 1 para o projeto 9.", new DateTime(2024, 10, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "EmAndamento", "Tarefa 1 do Projeto 9", 2L },
                    { 162L, null, "Descrição da tarefa 2 para o projeto 9.", new DateTime(2024, 9, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "Pendente", "Tarefa 2 do Projeto 9", 1L },
                    { 163L, null, "Descrição da tarefa 3 para o projeto 9.", new DateTime(2024, 10, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 9L, "EmAndamento", "Tarefa 3 do Projeto 9", 1L },
                    { 164L, null, "Descrição da tarefa 4 para o projeto 9.", new DateTime(2024, 9, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 9L, "Concluida", "Tarefa 4 do Projeto 9", 2L },
                    { 165L, null, "Descrição da tarefa 5 para o projeto 9.", new DateTime(2024, 9, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "EmAndamento", "Tarefa 5 do Projeto 9", 2L },
                    { 166L, null, "Descrição da tarefa 6 para o projeto 9.", new DateTime(2024, 11, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 9L, "EmAndamento", "Tarefa 6 do Projeto 9", 1L },
                    { 167L, null, "Descrição da tarefa 7 para o projeto 9.", new DateTime(2024, 11, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "Pendente", "Tarefa 7 do Projeto 9", 4L },
                    { 168L, null, "Descrição da tarefa 8 para o projeto 9.", new DateTime(2024, 9, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 9L, "Concluida", "Tarefa 8 do Projeto 9", 2L },
                    { 169L, null, "Descrição da tarefa 9 para o projeto 9.", new DateTime(2024, 9, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 9L, "Concluida", "Tarefa 9 do Projeto 9", 4L },
                    { 170L, null, "Descrição da tarefa 10 para o projeto 9.", new DateTime(2024, 10, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 22, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "EmAndamento", "Tarefa 10 do Projeto 9", 4L },
                    { 171L, null, "Descrição da tarefa 11 para o projeto 9.", new DateTime(2024, 10, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "Concluida", "Tarefa 11 do Projeto 9", 1L },
                    { 172L, null, "Descrição da tarefa 12 para o projeto 9.", new DateTime(2024, 9, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 9L, "Concluida", "Tarefa 12 do Projeto 9", 1L },
                    { 173L, null, "Descrição da tarefa 13 para o projeto 9.", new DateTime(2024, 12, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "Concluida", "Tarefa 13 do Projeto 9", 2L },
                    { 174L, null, "Descrição da tarefa 14 para o projeto 9.", new DateTime(2024, 10, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "Pendente", "Tarefa 14 do Projeto 9", 2L },
                    { 175L, null, "Descrição da tarefa 15 para o projeto 9.", new DateTime(2024, 10, 4, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 9L, "Concluida", "Tarefa 15 do Projeto 9", 4L },
                    { 176L, null, "Descrição da tarefa 16 para o projeto 9.", new DateTime(2024, 12, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 9L, "Pendente", "Tarefa 16 do Projeto 9", 2L },
                    { 177L, null, "Descrição da tarefa 17 para o projeto 9.", new DateTime(2024, 10, 13, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "EmAndamento", "Tarefa 17 do Projeto 9", 4L },
                    { 178L, null, "Descrição da tarefa 18 para o projeto 9.", new DateTime(2024, 9, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 9L, "Concluida", "Tarefa 18 do Projeto 9", 4L },
                    { 179L, null, "Descrição da tarefa 19 para o projeto 9.", new DateTime(2024, 11, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 14, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 9L, "Concluida", "Tarefa 19 do Projeto 9", 1L },
                    { 180L, null, "Descrição da tarefa 20 para o projeto 9.", new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 9L, "Concluida", "Tarefa 20 do Projeto 9", 3L },
                    { 181L, null, "Descrição da tarefa 1 para o projeto 10.", new DateTime(2024, 11, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Concluida", "Tarefa 1 do Projeto 10", 4L },
                    { 182L, null, "Descrição da tarefa 2 para o projeto 10.", new DateTime(2024, 11, 20, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "EmAndamento", "Tarefa 2 do Projeto 10", 4L },
                    { 183L, null, "Descrição da tarefa 3 para o projeto 10.", new DateTime(2024, 11, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 10L, "Pendente", "Tarefa 3 do Projeto 10", 3L },
                    { 184L, null, "Descrição da tarefa 4 para o projeto 10.", new DateTime(2024, 10, 6, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 10L, "Concluida", "Tarefa 4 do Projeto 10", 1L },
                    { 185L, null, "Descrição da tarefa 5 para o projeto 10.", new DateTime(2024, 12, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 27, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Concluida", "Tarefa 5 do Projeto 10", 1L },
                    { 186L, null, "Descrição da tarefa 6 para o projeto 10.", new DateTime(2024, 12, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 10L, "EmAndamento", "Tarefa 6 do Projeto 10", 1L },
                    { 187L, null, "Descrição da tarefa 7 para o projeto 10.", new DateTime(2024, 10, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Concluida", "Tarefa 7 do Projeto 10", 2L },
                    { 188L, null, "Descrição da tarefa 8 para o projeto 10.", new DateTime(2024, 11, 1, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 23, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 10L, "Concluida", "Tarefa 8 do Projeto 10", 3L },
                    { 189L, null, "Descrição da tarefa 9 para o projeto 10.", new DateTime(2024, 9, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 21, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 10L, "Concluida", "Tarefa 9 do Projeto 10", 1L },
                    { 190L, null, "Descrição da tarefa 10 para o projeto 10.", new DateTime(2024, 11, 15, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 9, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Pendente", "Tarefa 10 do Projeto 10", 1L },
                    { 191L, null, "Descrição da tarefa 11 para o projeto 10.", new DateTime(2024, 9, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Concluida", "Tarefa 11 do Projeto 10", 3L },
                    { 192L, null, "Descrição da tarefa 12 para o projeto 10.", new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 10L, "Concluida", "Tarefa 12 do Projeto 10", 1L },
                    { 193L, null, "Descrição da tarefa 13 para o projeto 10.", new DateTime(2024, 8, 30, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 8, 18, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Pendente", "Tarefa 13 do Projeto 10", 4L },
                    { 194L, null, "Descrição da tarefa 14 para o projeto 10.", new DateTime(2024, 11, 11, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 3, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "EmAndamento", "Tarefa 14 do Projeto 10", 1L },
                    { 195L, null, "Descrição da tarefa 15 para o projeto 10.", new DateTime(2024, 9, 28, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 17, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 10L, "Concluida", "Tarefa 15 do Projeto 10", 2L },
                    { 196L, null, "Descrição da tarefa 16 para o projeto 10.", new DateTime(2024, 11, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 29, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 10L, "Pendente", "Tarefa 16 do Projeto 10", 2L },
                    { 197L, null, "Descrição da tarefa 17 para o projeto 10.", new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 2, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Concluida", "Tarefa 17 do Projeto 10", 4L },
                    { 198L, null, "Descrição da tarefa 18 para o projeto 10.", new DateTime(2024, 11, 8, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 10, 26, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Alta", 10L, "Concluida", "Tarefa 18 do Projeto 10", 1L },
                    { 199L, null, "Descrição da tarefa 19 para o projeto 10.", new DateTime(2024, 11, 16, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 11, 10, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Media", 10L, "Concluida", "Tarefa 19 do Projeto 10", 1L },
                    { 200L, null, "Descrição da tarefa 20 para o projeto 10.", new DateTime(2024, 10, 5, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), new DateTime(2024, 9, 19, 23, 27, 29, 389, DateTimeKind.Utc).AddTicks(9625), "Baixa", 10L, "Pendente", "Tarefa 20 do Projeto 10", 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_UserId",
                table: "AuditLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserId",
                table: "Project",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ProjectId",
                table: "Task",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_UserId",
                table: "Task",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComment_TaskId",
                table: "TaskComment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComment_UserId",
                table: "TaskComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Name_Email",
                table: "User",
                columns: new[] { "Name", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserRoleId",
                table: "User",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Description",
                table: "UserRole",
                column: "Description",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "TaskComment");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
