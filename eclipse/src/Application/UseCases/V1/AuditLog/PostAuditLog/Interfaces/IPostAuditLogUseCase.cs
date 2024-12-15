using CrossCutting.Interfaces;

namespace Application.UseCases.V1.AuditLog.PostAuditLog.Interfaces;

public interface IPostAuditLogUseCase
{
    System.Threading.Tasks.Task Execute(
        Domain.Entities.AuditLog auditLog);

    void SetOutputPort(
        IOutputPort<Domain.Entities.AuditLog> outputPort);
}