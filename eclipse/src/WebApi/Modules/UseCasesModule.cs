using Application.UseCases.V1.AuditLog.PostAuditLog;
using Application.UseCases.V1.AuditLog.PostAuditLog.Interfaces;
using Application.UseCases.V1.Project.DeleteProject;
using Application.UseCases.V1.Project.DeleteProject.Interfaces;
using Application.UseCases.V1.Project.GetListAllProject;
using Application.UseCases.V1.Project.GetListAllProject.Interfaces;
using Application.UseCases.V1.Project.GetListSearchProject;
using Application.UseCases.V1.Project.GetListSearchProject.Interfaces;
using Application.UseCases.V1.Project.GetProjectById;
using Application.UseCases.V1.Project.GetProjectById.Interfaces;
using Application.UseCases.V1.Project.PostProject;
using Application.UseCases.V1.Project.PostProject.Interfaces;
using Application.UseCases.V1.Project.PutProject;
using Application.UseCases.V1.Project.PutProject.Interfaces;
using Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport;
using Application.UseCases.V1.Reports.GetListAverageTasksCompletedReport.Interfaces;
using Application.UseCases.V1.Reports.GetListDelayedProjectsReport;
using Application.UseCases.V1.Reports.GetListDelayedProjectsReport.Interfaces;
using Application.UseCases.V1.Reports.GetListProjectProgressReport;
using Application.UseCases.V1.Reports.GetListProjectProgressReport.Interfaces;
using Application.UseCases.V1.Reports.GetListTaskByPriorityReport.Interfaces;
using Application.UseCases.V1.Reports.GetListUserProductivityReport;
using Application.UseCases.V1.Reports.GetListUserProductivityReport.Interfaces;
using Application.UseCases.V1.Task.DeleteTask;
using Application.UseCases.V1.Task.DeleteTask.Interfaces;
using Application.UseCases.V1.Task.GetListAllTask;
using Application.UseCases.V1.Task.GetListAllTask.Interfaces;
using Application.UseCases.V1.Task.GetListSearchTask;
using Application.UseCases.V1.Task.GetListSearchTask.Interfaces;
using Application.UseCases.V1.Task.GetTaskById;
using Application.UseCases.V1.Task.GetTaskById.Interfaces;
using Application.UseCases.V1.Task.PostTask;
using Application.UseCases.V1.Task.PostTask.Interfaces;
using Application.UseCases.V1.Task.PutTask;
using Application.UseCases.V1.Task.PutTask.Interfaces;
using Application.UseCases.V1.TaskComment.DeleteTaskComment;
using Application.UseCases.V1.TaskComment.DeleteTaskComment.Interfaces;
using Application.UseCases.V1.TaskComment.GetListAllTaskComment;
using Application.UseCases.V1.TaskComment.GetListAllTaskComment.Interfaces;
using Application.UseCases.V1.TaskComment.GetListSearchTaskComment;
using Application.UseCases.V1.TaskComment.GetListSearchTaskComment.Interfaces;
using Application.UseCases.V1.TaskComment.GetTaskCommentById;
using Application.UseCases.V1.TaskComment.GetTaskCommentById.Interfaces;
using Application.UseCases.V1.TaskComment.PostTaskComment;
using Application.UseCases.V1.TaskComment.PostTaskComment.Interfaces;
using Application.UseCases.V1.TaskComment.PutTaskComment;
using Application.UseCases.V1.TaskComment.PutTaskComment.Interfaces;
using CrossCutting.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Modules;

public static class UseCasesModule
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddScoped<IDeleteProjectUseCase, DeleteProjectUseCase>();
        services.AddScoped<IGetListAllProjectUseCase, GetListAllProjectUseCase>();
        services.AddScoped<IGetListSearchProjectUseCase, GetListSearchProjectUseCase>();
        services.AddScoped<IGetProjectByIdUseCase, GetProjectByIdUseCase>();
        services.AddScoped<IPostProjectUseCase, PostProjectUseCase>();
        services.Decorate<IPostProjectUseCase, PostProjectValidationUseCase>();
        services.AddScoped<IPutProjectUseCase, PutProjectUseCase>();
        services.Decorate<IPutProjectUseCase, PutProjectValidationUseCase>();

        services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
        services.AddScoped<IGetListAllTaskUseCase, GetListAllTaskUseCase>();
        services.AddScoped<IGetListSearchTaskUseCase, GetListSearchTaskUseCase>();
        services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
        services.AddScoped<IPostTaskUseCase, PostTaskUseCase>();
        services.Decorate<IPostTaskUseCase, PostTaskValidationUseCase>();
        services.AddScoped<IPutTaskUseCase, PutTaskUseCase>();
        services.Decorate<IPutTaskUseCase, PutTaskValidationUseCase>();

        services.AddScoped<IDeleteTaskCommentUseCase, DeleteTaskCommentUseCase>();
        services.AddScoped<IGetListAllTaskCommentUseCase, GetListAllTaskCommentUseCase>();
        services.AddScoped<IGetListSearchTaskCommentUseCase, GetListSearchTaskCommentUseCase>();
        services.AddScoped<IGetTaskCommentByIdUseCase, GetTaskCommentByIdUseCase>();
        services.AddScoped<IPostTaskCommentUseCase, PostTaskCommentUseCase>();
        services.Decorate<IPostTaskCommentUseCase, PostTaskCommentValidationUseCase>();
        services.AddScoped<IPutTaskCommentUseCase, PutTaskCommentUseCase>();
        services.Decorate<IPutTaskCommentUseCase, PutTaskCommentValidationUseCase>();

        services.AddScoped<IPostAuditLogUseCase, PostAuditLogUseCase>();
        services.Decorate<IPostAuditLogUseCase, PostAuditLogValidationUseCase>();

        services.AddScoped<IGetListAverageTasksCompletedReportUseCase, GetListAverageTasksCompletedReportUseCase>();
        services.AddScoped<IGetListProjectProgressReportUseCase, GetListProjectProgressReportUseCase>();
        services.AddScoped<IGetListUserProductivityReportUseCase, GetListUserProductivityReportUseCase>();
        services.AddScoped<IGetListDelayedProjectsReportUseCase, GetListDelayedProjectsReportUseCase>();
        services.AddScoped<IGetListTaskByPriorityReportUseCase, GetListTaskByPriorityReportUseCase>();

        services.AddScoped<NotificationHelper, NotificationHelper>();

        return services;
    }
}