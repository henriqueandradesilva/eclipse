using Domain.Common.Enums;
using System;

namespace Domain.Entities;

public class AuditLog : BaseEntity
{
    public AuditLog()
    {

    }

    public AuditLog(
        long id,
        long entityId,
        long userId,
        string description,
        DateTime date,
        TypeEntityEnum entity) : base(id)
    {
        EntityId = entityId;
        UserId = userId;
        Description = description;
        Date = date;
        TypeEntity = entity;
    }

    public long EntityId { get; private set; }

    public long UserId { get; private set; }

    public string Description { get; private set; }

    private DateTime _date;
    public DateTime Date
    {
        get { return _date; }
        set { _date = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    public TypeEntityEnum TypeEntity { get; private set; }

    public virtual User User { get; private set; }
}