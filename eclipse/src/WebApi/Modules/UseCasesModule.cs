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

        services.AddScoped<IPostAuditLogUseCase, PostAuditLogUseCase>();
        services.Decorate<IPostAuditLogUseCase, PostAuditLogValidationUseCase>();

        services.AddScoped<IGetListProjectProgressReportUseCase, GetListProjectProgressReportUseCase>();
        services.AddScoped<IGetListUserProductivityReportUseCase, GetListUserProductivityReportUseCase>();
        services.AddScoped<IGetListDelayedProjectsReportUseCase, GetListDelayedProjectsReportUseCase>();
        services.AddScoped<IGetListTaskByPriorityReportUseCase, GetListTaskByPriorityReportUseCase>();

        services.AddScoped<NotificationHelper, NotificationHelper>();

        return services;
    }
}